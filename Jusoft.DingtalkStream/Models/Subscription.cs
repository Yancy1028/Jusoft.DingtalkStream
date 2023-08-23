using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream.Models
{
    /// <summary>
    /// 订阅信息
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// 默认 <code cef="SubscriptionType.EVENT">SubscriptionType.EVENT</code>
        /// 可选 EVENT、CALLBACK
        /// </summary>
        public string Type { get; set; } = SubscriptionType.EVENT;
        /// <summary>
        /// 订阅的具体业务topic，<br />
        /// - 事件类型统一填写*，<br />
        /// - 回调类型根据平台提供的topic填写，<br />
        ///     例如机器人回调为 /v1.0/im/bot/messages/get <br />
        ///     卡片回调为 /v1.0/card/instances/callback`
        /// </summary>
        public string Topic { get; set; }
    }
}
