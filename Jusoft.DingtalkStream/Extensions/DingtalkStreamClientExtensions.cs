using Jusoft.DingtalkStream.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream
{
    public static class DingtalkStreamClientExtensions
    {
        /// <summary>
        /// 注册事件订阅
        /// </summary>
        public static void RegisterEventSubscription(this DingtalkStreamClient client)
        {
            client.RegisterSubscription(SubscriptionType.EVENT, "*");
            Console.WriteLine("事件订阅【已注册】");
        }
        /// <summary>
        /// 注册机器人回调
        /// </summary>
        public static void RegisterIMRobotMessageCallback(this DingtalkStreamClient client)
        {
            client.RegisterSubscription(SubscriptionType.CALLBACK, SubscriptionTopic.IM_ROBOT_MESSAGE_GET);
            Console.WriteLine("机器人回调【已注册】");
        }
        /// <summary>
        /// 注册卡片回调
        /// </summary>
        public static void RegisterCardInstanceCallback(this DingtalkStreamClient client)
        {
            client.RegisterSubscription(SubscriptionType.CALLBACK, SubscriptionTopic.CARD_INSTANCE_CALLBACK);
            Console.WriteLine("卡片回调【已注册】");
        }
    }
}
