
using Jusoft.DingtalkStream;
using Jusoft.DingtalkStream.Internals;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Digntalk Stream 客户端处理服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IDingtalkStreamBuilder AddDingtalkStream(this IServiceCollection services, Action<DingtalkStreamOptions> Configuration)
        {
            services.Configure(Configuration);

            return new DingtalkStreamBuilder(services);
        }
        /// <summary>
        /// 添加 Digntalk Stream 客户端处理服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IDingtalkStreamBuilder AddDingtalkStream(this IServiceCollection services, IConfiguration configuration)
        {
            return AddDingtalkStream(services, options =>
            {
                options.ClientId = configuration["ClientId"];
                options.ClientSecret = configuration["ClientScript"];
                options.UA = configuration["UA"];
                foreach (var item in configuration.GetSection("Subscriptions").GetChildren())
                {
                    options.Subscriptions.Add(new Subscription
                    {
                        Topic = item["Topic"],
                        Type = item["Type"]
                    });
                }
                if (!string.IsNullOrWhiteSpace(configuration["AutoReplySystemMessage"]))
                {
                    options.AutoReplySystemMessage = Convert.ToBoolean(configuration["AutoReplySystemMessage"]);
                }
            });
        }
    }
}
