using Jusoft.DingtalkStream.Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Robot.Internals
{
    internal static class Utilities
    {
        /// <summary>
        /// 订阅机器人的 Topic
        /// </summary>
        public const string SUBSCRIPTION_TOPIC = "/v1.0/im/bot/messages/get";
        /// <summary>
        /// 订阅机器人的类型
        /// </summary>

        public const string SUBSCRIPTION_TYPE = SubscriptionType.CALLBACK;
    }
}
