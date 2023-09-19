using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream.Core
{
    /// <summary>
    /// Dingtalk Stream 数据包
    /// </summary>
    public class DingtalkStreamDataPackage
    {
        internal DingtalkStreamDataPackage(JsonElement payload)
        {
            this.Payload = payload;

            this.Headers = new DingtalkStreamDataHeaders(payload.GetProperty("headers"));
        }
        /// <summary>
        /// 消息的原始 JSON 数据
        /// </summary>
        public JsonElement Payload { get; }

        /// <summary>
        /// 协议版本号
        /// </summary>
        public string SpecVersion => this.Payload.GetProperty("specVersion").GetString();

        /// <summary>
        /// 推送数据类型，当前支持三种：
        /// <list type="bullet">
        ///     <item>SYSTEM: 系统数据</item>
        ///     <item>EVENT：事件推送</item>
        ///     <item>CALLBACK：回调推送</item>
        /// </list>
        /// </summary>
        public string Type => this.Payload.GetProperty("type").GetString();

        /// <summary>
        /// 服务端推送的数据的 Header 信息
        /// </summary>
        public DingtalkStreamDataHeaders Headers { get; }

        /// <summary>
        /// 推送的数据
        /// <para>数据内容格式参考 Headers 下的 ContentType </para>
        /// </summary>
        public string Data => this.Payload.GetProperty("data").GetString();
    }
}
