
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
        public static IServiceCollection AddDingtalkStream(this IServiceCollection services, Action<DingtalkStreamOptions> Configuration)
        {
            services.Configure(Configuration);
            services.AddSingleton<DingtalkStreamClient>();
            services.AddSingleton<DingtalkStreamMessageHandler, DefaultMessageHandler>();

            return services;
        }
        public static IServiceCollection AddDingtalkStream(this IServiceCollection services, IConfiguration configuration)
        {
            return AddDingtalkStream(services, options =>
            {
                options.ClientId = configuration["ClientId"];
                options.ClientSecret = configuration["ClientScript"];
                options.UA = configuration["ua"];
            });
        }
    }
}
