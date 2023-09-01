using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Core
{
    public static class SubscriptionTopic
    {
        /// <summary>
        /// 事件订阅所使用的 Topic
        /// </summary>
        public const string EVENT = "*";

        /// <summary>
        /// 卡片回调 Topic
        /// </summary>
        public const string CARD_INSTANCE_CALLBACK = "/v1.0/card/instances/callback";
    }
}
