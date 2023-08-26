using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream
{
    public static class SubscriptionTopic
    {
        /// <summary>
        /// 事件订阅所使用的 Topic
        /// </summary>
        public const string EVENT = "*";
        /// <summary>
        /// 机器人回调 Topic
        /// </summary>
        public const string IM_ROBOT_MESSAGE_GET = "/v1.0/im/robot/messages/get";

        /// <summary>
        /// 卡片回调 Topic
        /// </summary>
        public const string CARD_INSTANCE_CALLBACK = "/v1.0/card/instances/callback";
    }
}
