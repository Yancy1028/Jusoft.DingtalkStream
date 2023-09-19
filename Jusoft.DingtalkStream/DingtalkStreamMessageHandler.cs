using Jusoft.DingtalkStream.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream
{
    /// <summary>
    /// 消息处理程序
    /// </summary>
    public abstract class DingtalkStreamMessageHandler
    {
        #region 辅助创建应答消息的方法
        /// <summary>
        /// 创建应答消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected static async Task<byte[]> CreateReplyMessage(string messageId, string data)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();
            writer.WriteNumber("code", 200);

            writer.WritePropertyName("headers");
            writer.WriteStartObject();
            writer.WriteString("contentType", "application/json");
            writer.WriteString("messageId", messageId);
            writer.WriteEndObject();

            writer.WriteString("message", "OK");
            writer.WriteString("data", data);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return stream.ToArray();
        }
        /// <summary>
        /// 创建消费成功的事件推送的 回复消息
        /// </summary>
        /// <param name="customMessage">自定义消息</param>
        /// <returns></returns>
        protected static async Task<string> CreateReplySuccessEventMessageData(string customMessage)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("status", "SUCESS");
            writer.WriteString("message", customMessage);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        /// <summary>
        /// 创建消费失败的事件推送的 回复消息
        /// </summary>
        /// <param name="customMessage">自定义消息</param>
        /// <returns></returns>
        protected static async Task<string> CreateReplyFaildEventMessageData(string customMessage)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("status", "LATER");
            writer.WriteString("message", customMessage);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 创建回调推送的 回复消息
        /// </summary>
        /// <param name="responseJson">真实的业务响应JSON</param>
        /// <returns></returns>
        protected static async Task<string> CreateReplyCallbackMessageData(string responseJson)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            writer.WriteStartObject();

            writer.WriteString("response", responseJson);

            writer.WriteEndObject();
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        #endregion

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="messageType">数据类型</param>
        /// <param name="client">客户端</param>
        public virtual async Task HandleMessage(DingtalkStreamClient client, WebSocketMessageType messageType, byte[] message)
        {
            if (messageType == WebSocketMessageType.Text)
            {
                // 将内存流转换为字符串
                // 处理文字消息（异步处理）
                await OnMessage(client, Encoding.UTF8.GetString(message));
            }
            //! 目前暂时不会出现传出Binary 类型的消息
            //else if (messageType == WebSocketMessageType.Binary)
            //{
            // 处理二进制数据
            //await OnMessage(memoryStream);
            //}
        }

        /// <summary>
        /// 针对接收到的消息内容进行处理
        /// </summary>
        /// <returns></returns>
        async Task OnMessage(DingtalkStreamClient client, string message)
        {
            Console.WriteLine("收到消息：" + message);
            var payload = JsonDocument.Parse(message);

            var jsonElmSpecVersion = payload.RootElement.GetProperty("specVersion");// 协议版本
            var jsonElmType = payload.RootElement.GetProperty("type"); // 推送数据类型。SYSTEM: 系统数据; EVENT：事件推送; CALLBACK：回调推送
            var jsonElmData = payload.RootElement.GetProperty("data");// 推送数据内容

            var jsonElmHeaders = payload.RootElement.GetProperty("headers");// 提取headers
            var jsonElmMessageId = jsonElmHeaders.GetProperty("messageId"); // 推送消息ID，标记一次推送，客户端需要关注此信息并且在响应的时候将此信息回传给服务端
            var jsonElmTopic = jsonElmHeaders.GetProperty("topic"); // 推送的业务 Topic

            // 识别数据类型
            switch (jsonElmType.GetString())
            {
                // 钉钉系统侧推送的消息。例如探活消息
                case "SYSTEM":
                    {
                        switch (jsonElmTopic.GetString())
                        {
                            case "ping":// 探活
                                var replyMessageByts = await CreateReplyMessage(jsonElmMessageId.GetString(), jsonElmData.GetString());
                                await client.SendAsync(replyMessageByts, WebSocketMessageType.Text, true, CancellationToken.None);
                                break;
                            case "disconnect":
                                // 断连请求 topic 为 disconnect,， 收到断连请求后，连接将不会再有新的下行消息推送下来， 此期间客户端依然可以通过此连接响应正在处理的请求。 客户端收到断连请求之后，需要发起新的注册长连接和建连请求，原有的ticket无法复用。客户端不需要对此推送信息做任何响应， 服务端在静默10s之后会主动断开 tcp 连接。
                                //! 对于钉钉侧，可能因为各种未知原因，会主动断开连接，此时需要重新注册连接
                                await client.Restart("钉钉服务要求客户端进行重连");
                                return;
                        }
                    }
                    break;
                // 注册的事件回调
                case SubscriptionType.EVENT:
                    {

                    }
                    break;
                // 注册的其他回调
                case SubscriptionType.CALLBACK:
                    {

                    }
                    break;
                default:
                    break;
            }
        }
    }
}
