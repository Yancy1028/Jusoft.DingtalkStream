using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Internals
{
    internal class DingtalkStreamClientWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DingtalkStreamClientWorker> logger;

        public DingtalkStreamClientWorker(IServiceProvider serviceProvider, ILogger<DingtalkStreamClientWorker> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 延迟一小会儿再开始;
            await Task.Delay(1000, stoppingToken);
            try
            {
                // 创建 Dingtalk Stream Client
                var client = ActivatorUtilities.CreateInstance<DingtalkStreamClient>(serviceProvider);
                client.OnMessage += this.Client_OnMessage;
                // 启动
                await client.Start();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "订阅程序已停止运行。");
            }
        }

        private void Client_OnMessage(object sender, MessageEventHanderArgs e)
        {
            var handler = serviceProvider.GetRequiredService<IDingtalkStreamMessageHandler>();
            try
            {
                handler?.HandleMessage(e);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "处理消息的过程中发生了错误。");
            }
        }
    }
}
