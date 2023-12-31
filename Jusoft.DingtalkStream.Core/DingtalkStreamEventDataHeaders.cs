using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream.Core
{
    /// <summary>
    /// EVENT 事件推送的headers 信息
    /// </summary>
    public class DingtalkStreamEventDataHeaders : DingtalkStreamDataHeaders
    {
        public DingtalkStreamEventDataHeaders(JsonElement payload) : base(payload)
        { }

        /// <summary>
        /// 事件类型。
        /// </summary>
        public string EventType => this.Payload.GetProperty("eventType").GetString();
        /// <summary>
        /// 事件的唯一Id。
        /// </summary>
        public string EventId => this.Payload.GetProperty("eventId").GetString();
        /// <summary>
        /// 事件所属的corpId
        /// </summary>
        public string EventCorpId => this.Payload.GetProperty("eventCorpId").GetString();
        /// <summary>
        /// 事件生成时间（毫秒）
        /// </summary>
        public long EventBornTime => Convert.ToInt64(this.Payload.GetProperty("eventBornTime").GetString());// 钉钉返回的eventBornTime 是字符串类型，此处需要转换为long类型后进行使用
        /// <summary>
        /// 统一应用身份Id。
        /// </summary>
        public string EventUnifiedAppId => this.Payload.GetProperty("eventUnifiedAppId").GetString();
    }
}
