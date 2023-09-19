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
            builder.RegisterSubscription(SubscriptionType.EVENT, SubscriptionTopic.EVENT);

            return builder;
        }

        /// <summary>
        /// 注册卡片回调
        /// </summary>
        public static IDingtalkStreamBuilder RegisterCardInstanceCallback(this IDingtalkStreamBuilder builder)
        {
            builder.RegisterSubscription(SubscriptionType.CALLBACK, SubscriptionTopic.CARD_INSTANCE_CALLBACK);

            return builder;
        }

        /// <summary>
        /// 添加消息处理程序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IDingtalkStreamBuilder AddMessageHandler<T>(this IDingtalkStreamBuilder builder) where T : class, IDingtalkStreamMessageHandler
        {
            builder.Services.AddTransient<IDingtalkStreamMessageHandler, T>();

            return builder;
        }
    }
}
