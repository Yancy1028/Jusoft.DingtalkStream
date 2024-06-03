//#define LOCAL_DEBUG
using Jusoft.DingtalkStream.Core.Internals;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Core
{
    /// <summary>
    /// 钉钉Stream客户端
    /// </summary>
    public class DingtalkStreamClient : IDisposable
    {
        /// <summary>
        /// 请求接口使用的通道
        /// </summary>
        readonly static HttpClient Backchannel = new HttpClient();

        const int BUFFER_SIZE = 1024 * 4;
        const int TICKET_TIMEOUT_SECONDS = 90;

        readonly DingtalkStreamOptions options;
        readonly ILogger logger;
        ClientWebSocket webSocketClient;
        CancellationTokenSource cancellationTokenSource;

        public ClientWebSocket WebSocketClient => webSocketClient;



        /// <summary>
        /// 注册的订阅
        /// </summary>
        public ReadOnlyCollection<Subscription> Subscriptions => options.Subscriptions.AsReadOnly();
        /// <summary>
        /// 收到消息后的
        /// </summary>
        public event EventHandler<MessageEventHanderArgs> OnMessage;
        /// <summary>
        /// 当客户端启动后
        /// </summary>
        public event EventHandler OnStarted;
        /// <summary>
        /// 当客户端停止后
        /// </summary>
        public event EventHandler<Exception> OnStoped;


        public DingtalkStreamClient(IOptions<DingtalkStreamOptions> options , ILogger<DingtalkStreamClient> logger)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        /// <summary>
        /// 获取连接终结点
        /// </summary>
        /// <returns></returns>
        async Task<GetGatewayEndpointResponse> GetGatewayEndPoint()
        {
#if DEBUG && LOCAL_DEBUG
            var gatewayEndpointResponse = new GetGatewayEndpointResponse();

            gatewayEndpointResponse.Ticket = "12345";
            gatewayEndpointResponse.EndPoint = "ws://localhost:12345";

            return await Task.FromResult(gatewayEndpointResponse);
#else
            Throws.IfNullOrWhiteSpace(this.options.ClientId , nameof(this.options.ClientId));
            Throws.IfNullOrWhiteSpace(this.options.ClientSecret , nameof(this.options.ClientSecret));
            Throws.IfEmptyArray(this.Subscriptions , "尚未注册任何订阅。请先通过RegisterSubscription 进行事件订阅。");

            logger.LogInformation("开始请求钉钉访问网关及凭据。");

            #region 构造请求用的JSON 参数
            var jsonContentStr = string.Empty;

            using (var stream = new MemoryStream())
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteStartObject();
                writer.WriteString("clientId" , this.options.ClientId);
                writer.WriteString("clientSecret" , this.options.ClientSecret);
                // 生成订阅信息
                writer.WritePropertyName("subscriptions");
                writer.WriteStartArray();

                // 针对 EVENT 的类型，仅需要进行一次订阅即可
                if (this.Subscriptions.Any(a => a.Type == "EVENT"))
                {
                    writer.WriteStartObject();
                    writer.WriteString("type" , "EVENT");
                    writer.WriteString("topic" , "*");
                    writer.WriteEndObject();
                }
                // 针对非 EVENT 的类型，需要明确订阅的 topic 信息
                foreach (var subscription in this.Subscriptions.Where(w => w.Type != "EVENT"))
                {
                    writer.WriteStartObject();
                    writer.WriteString("type" , subscription.Type);
                    writer.WriteString("topic" , subscription.Topic);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                // 生成 UA 信息
                var userAgent = $"{Utilities.GetOSVersion()} {Utilities.GetFrameworkVersion()} {Utilities.GetSDKVersion()}";
                if (!string.IsNullOrWhiteSpace(options.UA))
                {
                    userAgent = $"{userAgent} {options.UA}";
                }
                writer.WriteString("ua" , userAgent);

                // 将本地的多个IP 组合写入
                var localIp = string.Join(',' , Utilities.GetLocalIps());
                if (string.IsNullOrWhiteSpace(localIp))
                {
                    writer.WriteString("localIp" , "Unknown");
                }
                else
                {
                    writer.WriteString("localIp" , localIp);
                }
                writer.WriteEndObject();

                await writer.FlushAsync();

                jsonContentStr = Encoding.UTF8.GetString(stream.GetBuffer());
            }
            logger.LogDebug("请求地址：{}\r\n请求方式：POST\r\nBody：{}" , Utilities.DINGTALK_GATEWAY_ENDPOINT , jsonContentStr);

            #endregion
            var requestBodyContent = new StringContent(jsonContentStr , Encoding.UTF8 , mediaType: "application/json");

            var response = await Backchannel.PostAsync(Utilities.DINGTALK_GATEWAY_ENDPOINT , requestBodyContent);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogInformation("钉钉网关及凭据请求失败。");
                Throws.InternalException("钉钉网关及凭据请求失败。" + await response.Content.ReadAsStringAsync());
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            logger.LogInformation("钉钉网关及凭据请求成功");
            logger.LogDebug("网关及凭据信息：{}" , responseContent);

            var payload = JsonDocument.Parse(responseContent);

            var gatewayEndpointResponse = new GetGatewayEndpointResponse();

            if (payload.RootElement.TryGetProperty("endpoint" , out JsonElement jsonElmEndPoint))
            {
                gatewayEndpointResponse.EndPoint = jsonElmEndPoint.GetString();
            }
            if (payload.RootElement.TryGetProperty("ticket" , out JsonElement jsonElmTicket))
            {
                gatewayEndpointResponse.Ticket = jsonElmTicket.GetString();
            }

            return gatewayEndpointResponse;
#endif
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            logger.LogInformation("开始启动新的【钉钉Stream客户端】。");
            var cancellationTokenSource = new CancellationTokenSource();
            var ticketTimeoutSeconds = TICKET_TIMEOUT_SECONDS - 10;// ticket 过期时长的偏移值。

            Stopwatch sw = new Stopwatch();// 计时器
            int retry = 0; //重试次数

            Uri endPoint = null;
            do
            {
                try
                {
                    if (endPoint == null || sw.Elapsed.TotalSeconds > ticketTimeoutSeconds)
                    {
                        // 重连期间如果超过ticket有效期时还未连接成功，则重新获取ticket
                        // 获取钉钉回调网关连接信息
                        var gatewayEndpointResponse = await GetGatewayEndPoint();
                        endPoint = gatewayEndpointResponse.ToUri();
                        sw.Restart();// 重新计时
                    }
                    // 生成websocket 连接地址
                    logger.LogInformation("请求连接钉钉网关。");

                    // 创建新的连接客户端
                    this.webSocketClient = new ClientWebSocket();
                    // 进行连接
                    await this.webSocketClient.ConnectAsync(endPoint , cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex , "建立 WebSocket 连接时发生了异常。");
                    logger.LogInformation($"尝试第{++retry}次重连。");
                }
            } while (this.webSocketClient.State != WebSocketState.Open);
            sw.Stop();

            logger.LogInformation("钉钉网关 连接成功。");
            // 开始接收消息
            //_ = Task.Run(ReceiveHandler , cancellationTokenSource.Token);
            _ = Task.Factory.StartNew(ReceiveHandler , cancellationTokenSource.Token , TaskCreationOptions.LongRunning , TaskScheduler.Default);

            this.cancellationTokenSource = cancellationTokenSource;

            OnStarted?.Invoke(this , EventArgs.Empty);

            logger.LogInformation("【钉钉Stream客户端】已成功启动。");

            //=========================> 以下为内部函数

            /// <summary>
            /// 接收消息处理程序
            /// </summary>
            /// <returns></returns>
            async Task ReceiveHandler()
            {
                var buffer = new byte[BUFFER_SIZE];// 缓存

                while (!cancellationTokenSource.IsCancellationRequested && webSocketClient.State == WebSocketState.Open)
                {
                    try
                    {
                        WebSocketReceiveResult result;// 接收结果
                        WebSocketMessageType messageType; // 记录消息类型
                        using var memoryStream = new MemoryStream();// 缓存接收到的消息数据
                        do
                        {
                            result = await webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer) , cancellationTokenSource.Token);

                            messageType = result.MessageType;
                            // 判断是否关闭了消息传输
                            if (messageType == WebSocketMessageType.Close)
                            {
                                logger.LogInformation("接收到了关闭消息传输的指令。");
                                return;

                                //Throws.InternalException("接收到了关闭消息传输的指令。" + result.CloseStatusDescription);
                            }
                            // 写入内存流
                            await memoryStream.WriteAsync(buffer.AsMemory(0 , result.Count) , cancellationTokenSource.Token);
                        } while (!cancellationTokenSource.IsCancellationRequested && !result.EndOfMessage);
                        // 设置内存流指针到开始位置
                        memoryStream.Seek(0 , SeekOrigin.Begin);

                        _ = MessageHandler(messageType , memoryStream.ToArray());
                    }
                    catch (WebSocketException ex)
                    {
                        if (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                        {
                            logger.LogWarning(ex , "与钉钉服务器断开连接，请检查您的网络是否发生异常，也可能钉钉Stream服务端正在更新重启。");
                            await this.Restart("与钉钉服务器断开连接，请检查您的网络是否发生异常，也可能钉钉Stream服务端正在更新重启。");
                            return;
                        }
                        else
                        {
                            logger.LogError(ex , "接收消息时发生了异常。");
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex , "接收消息时发生了异常。");
                        continue;
                    }
                }
            }

            /// <summary>
            /// 针对消息体进行处理
            /// </summary>
            /// <param name="socketMessageType"></param>
            /// <param name="messageByts"></param>
            /// <returns></returns>
            async Task MessageHandler(WebSocketMessageType socketMessageType , byte[] messageByts)
            {
                try
                {
                    switch (socketMessageType)
                    {
                        case WebSocketMessageType.Text:
                            {
                                var payload = JsonDocument.Parse(messageByts);
                                logger.LogDebug("收到消息：{}" , payload.RootElement.ToString());

                                var jsonElmSpecVersion = payload.RootElement.GetProperty("specVersion");// 协议版本
                                var jsonElmType = payload.RootElement.GetProperty("type"); // 推送数据类型。SYSTEM: 系统数据; EVENT：事件推送; CALLBACK：回调推送
                                var jsonElmHeaders = payload.RootElement.GetProperty("headers");// 提取headers
                                var jsonElmMessageId = jsonElmHeaders.GetProperty("messageId"); // 推送消息ID，标记一次推送，客户端需要关注此信息并且在响应的时候将此信息回传给服务端

                                // 是否针对消息已进行了处理
                                var isHandled = false;
                                // 判断是否内部接管自动回复系统消息的处理
                                if (options.AutoReplySystemMessage && jsonElmType.GetString() == "SYSTEM")
                                {
                                    isHandled = await OnSystemMessage(payload);
                                }

                                // 判断消息是否未处理，针对所有未处理的消息转交到 OnMessage 事件中进行处理。
                                if (!isHandled)
                                {
                                    // 触发消息事件
                                    OnMessage?.Invoke(this , new MessageEventHanderArgs(payload.RootElement , webSocketClient));
                                }
                            }
                            return;
                        case WebSocketMessageType.Binary:
                            logger.LogWarning("收到二进制类型的消息。SDK 暂不处理");
                            return;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex , "处理消息时发生了异常。");
                }
            }

            /// <summary>
            /// 仅针对消息类型为SYSTEM 的时候进行的处理
            /// </summary>
            /// <param name="payload"></param>
            /// <returns></returns>
            async Task<bool> OnSystemMessage(JsonDocument payload)
            {
                var jsonElmData = payload.RootElement.GetProperty("data");// 推送数据内容

                var jsonElmHeaders = payload.RootElement.GetProperty("headers");// 提取headers
                var jsonElmMessageId = jsonElmHeaders.GetProperty("messageId"); // 推送消息ID，标记一次推送，客户端需要关注此信息并且在响应的时候将此信息回传给服务端
                var jsonElmTopic = jsonElmHeaders.GetProperty("topic"); // 推送的业务 Topic

                switch (jsonElmTopic.GetString())
                {
                    case "ping":// 探活
                        var replyMessageByts = await DingtalkStreamUtilities.CreateReplyMessage(jsonElmMessageId.GetString() , jsonElmData.GetString());
                        logger.LogDebug("回复消息：{}" , Encoding.UTF8.GetString(replyMessageByts));
                        await webSocketClient.SendAsync(replyMessageByts , WebSocketMessageType.Text , true , CancellationToken.None);
                        return true;
                    case "disconnect":
                        // 断连请求 topic 为 disconnect,， 收到断连请求后，连接将不会再有新的下行消息推送下来， 此期间客户端依然可以通过此连接响应正在处理的请求。 客户端收到断连请求之后，需要发起新的注册长连接和建连请求，原有的ticket无法复用。客户端不需要对此推送信息做任何响应， 服务端在静默10s之后会主动断开 tcp 连接。
                        //! 对于钉钉侧，可能因为各种未知原因，会主动断开连接，此时需要重新注册连接
                        await this.Restart("钉钉Stream服务端要求客户端进行重连");
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 重启订阅服务
        /// </summary>
        /// <param name="statusDescription">重启的原因或信息</param>
        /// <returns></returns>
        public async Task Restart(string statusDescription)
        {
            try
            {
                logger.LogInformation("即将重启【钉钉Stream客户端】。原因：{}" , statusDescription);

                this.cancellationTokenSource.Cancel();// 取消正在进行的一些异步任务；
                if (this.webSocketClient.State == WebSocketState.Open)
                {
                    logger.LogInformation("正在关闭与【钉钉Stream】服务的连接。");

                    await Task.Yield();// 尽可能让其他任务在完毕之前快点完成
                    // 主动断开连接
                    await this.webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure , statusDescription , CancellationToken.None);
                    // 终止 webSocketClient 的 IO 操作
                    await Task.Yield();// 尽可能在断开IO 操作之前，让其他任务快点完成
                    this.webSocketClient.Abort();
                    this.webSocketClient.Dispose(); // 释放资源
                    GC.Collect();// 强制回收资源
                }
                await Start();
            }
            catch (Exception ex)
            {
                logger.LogError(ex , "重启【钉钉Stream客户端】时发生了异常，程序已停止。");
                OnStoped?.Invoke(this , ex);
            }
        }

        public void Dispose()
        {
            webSocketClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
