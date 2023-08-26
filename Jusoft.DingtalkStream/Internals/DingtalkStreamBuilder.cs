using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Jusoft.DingtalkStream.Internals
{
    internal class DingtalkStreamBuilder : IDingtalkStreamBuilder
    {
        public IServiceCollection Services { get; }

        public DingtalkStreamBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IDingtalkStreamBuilder RegisterSubscription([NotNull] string type, [NotNull] string topic)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentNullException(nameof(topic));

            Services.Configure<DingtalkStreamOptions>(options =>
            {
                if (!options.Subscriptions.Any(x => x.Type == type && x.Topic == topic))
                {
                    // 不存在订阅时才进行订阅添加
                    options.Subscriptions.Add(new Subscription { Type = type, Topic = topic });
                }
            });

            return this;
        }
        public IDingtalkStreamBuilder AddHostServices()
        {
            Services.AddHostedService<DingtalkStreamClientWorker>();
            return this;
        }
    }
}
