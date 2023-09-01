using Jusoft.DingtalkStream.Robot.Internals;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Core
{
    public static class IDingtalkStreamBuilderExtensions
    {

        /// <summary>
        /// 注册机器人回调
        /// </summary>
        public static IDingtalkStreamBuilder RegisterIMRobotMessageCallback(this IDingtalkStreamBuilder builder)
        {
            builder.RegisterSubscription(Utilities.SUBSCRIPTION_TYPE, Utilities.SUBSCRIPTION_TOPIC);

            return builder;
        }
    }
}
