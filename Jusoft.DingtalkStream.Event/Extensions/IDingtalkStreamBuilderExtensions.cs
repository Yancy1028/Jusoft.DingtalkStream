using Jusoft.DingtalkStream.Event.Internals;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Core
{
    public static class IDingtalkStreamBuilderExtensions
    {
        /// <summary>
        /// 注册事件订阅
        /// </summary>
        public static IDingtalkStreamBuilder RegisterEventSubscription(this IDingtalkStreamBuilder builder)
        {
            builder.RegisterSubscription(Utilities.SUBSCRIPTION_TYPE, Utilities.SUBSCRIPTION_TOPIC);

            return builder;
        }
    }
}
