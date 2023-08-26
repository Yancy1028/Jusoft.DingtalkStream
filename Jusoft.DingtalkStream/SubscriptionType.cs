using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream
{
    public static class SubscriptionType
    {
        /// <summary>
        /// 系统事件（无需订阅）
        /// </summary>
        public const string SYSTEM = "SYSTEM";
        /// <summary>
        /// 事件类型
        /// </summary>
        public const string EVENT = "EVENT";
        /// <summary>
        /// 回调类型
        /// </summary>
        public const string CALLBACK = "CALLBACK";
    }
}
