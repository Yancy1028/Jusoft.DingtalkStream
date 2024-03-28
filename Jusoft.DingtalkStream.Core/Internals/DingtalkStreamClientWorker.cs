using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Core.Internals
{
    internal class DingtalkStreamClientWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DingtalkStreamClientWorker> logger;
        readonly TaskFactory taskFactory;

        public DingtalkStreamClientWorker(IServiceProvider serviceProvider , ILogger<DingtalkStreamClientWorker> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            // 任务调度器
            var lcts = new LimitedConcurrencyLevelTaskScheduler(Environment.ProcessorCount);
            // 任务实例管理器
            this.taskFactory = new TaskFactory(lcts);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 延迟一小会儿再开始;
            await Task.Delay(1000 , stoppingToken);
            try
            {
                // 创建 Dingtalk Stream Client
                var client = ActivatorUtilities.CreateInstance<DingtalkStreamClient>(serviceProvider);
                client.OnMessage += (sender , e) =>
                {
                    this.taskFactory.StartNew(async (data) =>
                    {
                        try
                        {
                            using var scope = serviceProvider.CreateScope();
                            var handler = scope.ServiceProvider.GetRequiredService<IDingtalkStreamMessageHandler>();
                            await handler?.HandleMessage((MessageEventHanderArgs)data);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex , "处理消息的过程中发生了错误。");
                        }
                    } , e , stoppingToken);
                };
                // 启动
                await client.Start();
            }
            catch (Exception ex)
            {
                logger.LogError(ex , "订阅程序已停止运行。");
            }
        }

    }
}
