using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream
{
    /// <summary>
    /// Dingtalk Stream 的一些辅助方法
    /// </summary>
    public static class DingtalkStreamUtilities
    {

        #region 辅助创建应答消息的方法
        /// <summary>
        /// 创建应答消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> CreateReplyMessage(string messageId, string data)
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
        public static async Task<string> CreateReplyEventSuccessMessageData(string customMessage)
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
        public static async Task<string> CreateReplyEventFaildMessageData(string customMessage)
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
        public static async Task<string> CreateReplyCallbackMessageData(string responseJson)
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

    }
}
