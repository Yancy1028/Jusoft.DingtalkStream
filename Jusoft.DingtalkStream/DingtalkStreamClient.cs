using Jusoft.DingtalkStream.Internals;
using Jusoft.DingtalkStream.Models;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream
{
    /// <summary>
    /// 钉钉Stream客户端
    /// </summary>
    public class DingtalkStreamClient : IDisposable
    {
        readonly DingtalkStreamOptions options;
        readonly DingtalkStreamMessageHandler messageHandler;
        readonly ClientWebSocket webSocketClient = new ClientWebSocket();
        readonly List<Subscription> subscriptions = new List<Subscription>();

        /// <summary>
        /// 添加的订阅消息
        /// </summary>
        public ReadOnlyCollection<Subscription> Subscriptions => subscriptions.AsReadOnly();

        public DingtalkStreamClient(IOptions<DingtalkStreamOptions> options, DingtalkStreamMessageHandler dingtalkStreamMessageHandler) : this(options.Value, dingtalkStreamMessageHandler) { }

        /// <summary>
        /// 推荐使用注入方式
        /// </summary>
        /// <param name="options"></param>
        public DingtalkStreamClient(DingtalkStreamOptions options, DingtalkStreamMessageHandler dingtalkStreamMessageHandler)
        {
            this.options = options;
            this.messageHandler = dingtalkStreamMessageHandler;
        }

        /// <summary>
        /// 获取连接终结点
        /// </summary>
        /// <returns></returns>
        async Task<GetGatewayEndpointResponse> GetGatewayEndPoint()
        {
            if (subscriptions.Count == 0)
            {
                throw new DingtalkStreamException("尚未注册任何订阅。请先通过RegisterSubscription 进行事件订阅。");
            }

            using var httpHandler = new HttpClient();

            #region 构造请求用的JSON 参数
            var currentVersion = typeof(DingtalkStreamClient).Assembly.GetName().Version;

            var jsonContentStr = string.Empty;

            using (var stream = new MemoryStream())
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteStartObject();
                writer.WriteString("clientId", this.options.ClientId);
                writer.WriteString("clientSecret", this.options.ClientSecret);
                // 生成订阅信息
                writer.WritePropertyName("subscriptions");
                writer.WriteStartArray();

                // 针对 EVENT 的类型，仅需要进行一次订阅即可
                if (subscriptions.Any(a => a.Type == "EVENT"))
                {
                    writer.WriteStartObject();
                    writer.WriteString("type", "EVENT");
                    writer.WriteString("topic", "*");
                    writer.WriteEndObject();
                }
                // 针对非 EVENT 的类型，需要明确订阅的 topic 信息
                foreach (var subscription in subscriptions.Where(w => w.Type != "EVENT"))
                {
                    writer.WriteStartObject();
                    writer.WriteString("type", subscription.Type);
                    writer.WriteString("topic", subscription.Topic);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                // 生成 UA 信息
                var userAgent = $"{Utilities.GetSDKVersion()} {Utilities.GetOSVersion()} {Utilities.GetOSVersion()}";
                if (!string.IsNullOrWhiteSpace(options.UA))
                {
                    userAgent = $"{userAgent} {options.UA}";
                }
                writer.WriteString("ua", userAgent);
                // 获取第一个使用的本地IP
                var localIp = Utilities.GetLocalIps().FirstOrDefault();
                if (localIp != null)
                {
                    writer.WriteString("localIp", localIp.ToString());
                }
                writer.WriteEndObject();

                await writer.FlushAsync();

                jsonContentStr = Encoding.UTF8.GetString(stream.GetBuffer());
            }
            #endregion
            var requestBodyContent = new StringContent(jsonContentStr, Encoding.UTF8, mediaType: "application/json");

            var response = await httpHandler.PostAsync(Utilities.DINGTALK_GATEWAY_ENDPOINT, requestBodyContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new DingtalkStreamException(await response.Content.ReadAsStringAsync());
            }

            var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            GetGatewayEndpointResponse gatewayEndpointResponse = new GetGatewayEndpointResponse();

            if (payload.RootElement.TryGetProperty("endpoint", out JsonElement jsonElmEndPoint))
            {
                gatewayEndpointResponse.EndPoint = jsonElmEndPoint.GetString();
            }
            if (payload.RootElement.TryGetProperty("ticket", out JsonElement jsonElmTicket))
            {
                gatewayEndpointResponse.Ticket = jsonElmTicket.GetString();
            }

            return gatewayEndpointResponse;
        }

        /// <summary>
        /// 注册订阅。当订阅类型为 EVENT 时，此处 topic 的值可以参考 eventType 的值。例：企业增加员工事件【user_add_org】
        /// </summary>
        /// <param name="type">订阅类型。当前可选值：EVENT、CALLBACK</param>
        /// <param name="topic">对应 topic 的值，当填写为EVENT 时，可填写 eventType 的值，收到消息时自动触发callback 的处理</param>
        /// <param name="callback">当程序收到对应 type + topic 的匹配时，会执行 callback 回调处理程序。 </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void RegisterSubscription([NotNull] string type, [NotNull] string topic)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }
            // 此处暂时不做其他检查，方便订阅的拓展
            if (subscriptions.Any(x => x.Type == type && x.Topic == topic))
            {
                throw new ArgumentException($"已经存在相同的订阅信息:{type}（{topic}）");
            }
            subscriptions.Add(new Subscription { Type = type, Topic = topic });

        }

        // 创建websocket 客户端
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            // 获取钉钉回调网关连接信息
            var gatewayEndpointResponse = await GetGatewayEndPoint();

            // 生成websocket 连接地址
            var endPoint = gatewayEndpointResponse.ToUri();
            // 进行连接
            await webSocketClient.ConnectAsync(endPoint, CancellationToken.None);

            if (webSocketClient.State != WebSocketState.Open)
            {
                throw new DingtalkStreamException("连接钉钉回调网关失败。连接地址：" + endPoint);
            }
            // 开始接收消息
            _ = Task.Run(ReceiveHandler);
        }
        /// <summary>
        /// 重启订阅服务
        /// </summary>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        public async Task Restart(string statusDescription)
        {
            // 主动断开连接
            if (webSocketClient.State == WebSocketState.Open)
            {
                await webSocketClient.CloseAsync(WebSocketCloseStatus.Empty, statusDescription, CancellationToken.None);
            }
            await Start();
        }

        /// <summary>
        /// 发送消息到服务端
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="messageType"></param>
        /// <param name="endOfMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            return this.webSocketClient.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        /// <summary>
        /// 发送消息到服务端
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="messageType"></param>
        /// <param name="endOfMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            return this.webSocketClient.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        /// <summary>
        /// 接收消息处理程序
        /// </summary>
        /// <returns></returns>
        async Task ReceiveHandler()
        {
            var buffer = new byte[1024 * 4];// 缓存

            while (webSocketClient.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;// 接收结果
                WebSocketMessageType messageType; // 记录消息类型
                using MemoryStream memoryStream = new MemoryStream();// 缓存接收到的消息数据
                do
                {
                    result = await webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    messageType = result.MessageType;
                    // 判断是否关闭了消息传输
                    if (messageType == WebSocketMessageType.Close)
                    {
                        throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, result.CloseStatusDescription);
                    }
                    // 写入内存流
                    await memoryStream.WriteAsync(buffer, 0, result.Count);
                } while (!result.EndOfMessage);
                // 设置内存流指针到开始位置
                memoryStream.Seek(0, SeekOrigin.Begin);

                _ = this.messageHandler.HandleMessage(this, result.MessageType, memoryStream.ToArray());
            }

        }

        public void Dispose()
        {
            webSocketClient.Dispose();
        }
    }
}
