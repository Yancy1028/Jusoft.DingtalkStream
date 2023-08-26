using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jusoft.DingtalkStream
{
    public class DingtalkStreamOptions
    {
        readonly static AssemblyName EntryAssemblyName = Assembly.GetEntryAssembly().GetName();
        /// <summary>
        /// 三方应用: SuiteKey <br/>
        /// 企业自建：Appkey
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 三方应用：SuiteSecret <br/>
        /// 企业自建：AppSecret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 请求时所使用的 UA 信息,
        /// 默认为：当前应用程序版本信息
        /// </summary>
        public string UA { get; set; } = $"{EntryAssemblyName.Name}/{EntryAssemblyName.Version}";

        /// <summary>
        /// 订阅
        /// </summary>
        public List<Subscription> Subscriptions { get; } = new List<Subscription>();

        /// <summary>
        /// 自动回复钉钉的系统消息(ping,disconnect)
        /// </summary>
        public bool AutoReplySystemMessage { get; set; }
    }
}
