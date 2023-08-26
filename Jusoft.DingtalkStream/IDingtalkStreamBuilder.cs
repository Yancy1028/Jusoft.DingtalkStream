using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Jusoft.DingtalkStream
{
    public interface IDingtalkStreamBuilder
    {
        IServiceCollection Services { get; }

        /// <summary>
        /// 注册订阅。
        /// </summary>
        /// <param name="type">订阅类型。当前可选值：EVENT、CALLBACK</param>
        /// <param name="topic">对应 topic 的值，当填写为EVENT 时，可填写 eventType 的值，收到消息时自动触发callback 的处理</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        IDingtalkStreamBuilder RegisterSubscription([NotNull] string type, [NotNull] string topic);

        /// <summary>
        /// 添加到 HostServices （添加后会自动运行）
        /// </summary>
        /// <returns></returns>
        IDingtalkStreamBuilder AddHostServices();
    }
}
