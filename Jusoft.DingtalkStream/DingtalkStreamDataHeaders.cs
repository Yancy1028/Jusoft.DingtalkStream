using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream
{
    /// <summary>
    /// 钉钉数据信息头
    /// </summary>
    public class DingtalkStreamDataHeaders
    {
        public DingtalkStreamDataHeaders(JsonElement payload)
        {
            this.Payload = payload;
        }

        /// <summary>
        /// 消息的原始 JSON 数据
        /// </summary>
        public JsonElement Payload { get; }

        /// <summary>
        /// 本次推送的业务 Topic
        /// </summary>
        public string Topic => this.Payload.GetProperty("topic").GetString();

        /// <summary>
        /// 推送消息ID，标记一次推送，客户端需要关注此信息并且在响应的时候将此信息回传给服务端
        /// </summary>
        public string MessageId => this.Payload.GetProperty("messageId").GetString();

        /// <summary>
        /// 标记推送数据的格式，默认为application/json，代表推送的数据为一个json字符串；该字段预留将来支持二进制数据；
        /// </summary>
        public string ContentType => this.Payload.GetProperty("contentType").GetString();

        /// <summary>
        /// 推送时间,为unix时间戳，单位：毫秒
        /// </summary>
        public long Time => this.Payload.GetProperty("time").GetInt64();
    }
}
