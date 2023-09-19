using Jusoft.DingtalkStream.Robot.Internals;

using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Core
{
    public static class DingtalkStreamDataHeadersExtentions
    {
        /// <summary>
        /// 判断是否 Robot 的 Topic Headers
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static bool IsRobotTopic(this DingtalkStreamDataHeaders headers)
        {
            return headers.Topic == Utilities.SUBSCRIPTION_TOPIC;
        }
    }
}
