using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jusoft.DingtalkStream.Core
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

        public Action<DingtalkStreamClient> OnStarted { get; set; }
        public Action<DingtalkStreamClient, Exception> OnStoped { get; set; }

        /// <summary>
        /// 自动回复钉钉的系统消息(ping,disconnect)
        /// </summary>
        public bool AutoReplySystemMessage { get; set; }

        /// <summary>
        /// 最大并行任务数量，默认取CPU 核心数
        /// </summary>
        public int MaxTaskCount { get; set; } = Environment.ProcessorCount;
        /// <summary>
        /// 队列最大长度：默认 1000
        /// </summary>
        public int MaxQueueCount { get; set; } = 1000;
        /// <summary>
        /// 单次检测新数据的时间：默认5分钟
        /// </summary>
        public TimeSpan TimeInterval { get; set; } = TimeSpan.FromMinutes(5);
        /// <summary>
        /// 单个任务执行超时时间：默认5分钟
        /// </summary>
        public TimeSpan SingleExecuteTimeOut { get; set; } = TimeSpan.FromMinutes(5);
        /// <summary>
        /// 观察的任务执行时间的参考最近执行的任务数
        /// </summary>
        public int RecentExecutionTimeCount { get; set; } = 100;
    }
}
