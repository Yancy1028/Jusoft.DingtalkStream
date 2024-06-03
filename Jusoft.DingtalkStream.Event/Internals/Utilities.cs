using Jusoft.DingtalkStream.Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Event.Internals
{
    internal static class Utilities
    {
        /// <summary>
        /// 订阅事件消息的 Topic
        /// </summary>
        public const string SUBSCRIPTION_TOPIC = "*";
        /// <summary>
        /// 订阅事件消息的类型
        /// </summary>
        public const string SUBSCRIPTION_TYPE = SubscriptionType.EVENT;


        /// <summary>
        /// OA 审批的订阅 Topic
        ///
        /// 针对某个业务分类下特定审批模板的实例开始、结束或终止事件进行订阅。
        /// </summary>
        public const string SUBSCRIPTION_OA_TOPIC = "/v1.0/event/bpms_instance_change/bizCategoryId/{bizCategoryId}/processCode/{processCode}/type/{type}";

        /// <summary>
        /// 针对某个审批模板下的实例开始、结束或终止事件进行订阅。
        /// </summary>
        public const string SUBSCRIPTION_OA2_TOPIC = "/v1.0/event/bpms_instance_change/processCode/{processCode}/type/{type}";
    }
}
