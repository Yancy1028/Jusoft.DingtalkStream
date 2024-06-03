using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        //readonly TaskFactory taskFactory;

        IOptions<DingtalkStreamOptions> lazyOptions;
        // 待处理的数据队列
        readonly ConcurrentQueue<MessageEventHanderArgs> dataQueue = new ConcurrentQueue<MessageEventHanderArgs>();
        // 正在进行处理的数据集合
        readonly ConcurrentDictionary<string , MessageEventHanderArgs> activeTaskData;
        // 信号控制器
        readonly Lazy<SemaphoreSlim> lazySemaphoreSlim;

        protected DingtalkStreamOptions Options => lazyOptions.Value;
        protected SemaphoreSlim SmaphoreSlim => lazySemaphoreSlim.Value;
        /// <summary>
        /// 最大并发任务数
        /// </summary>
        protected int MaxConcurrentTask => this.Options.MaxTaskCount;

        // 记录任务最近的执行时间，利用滑动窗口大小来计算平均执行时间更为准确
        readonly ConcurrentQueue<TimeSpan> recentExecutionTimes = new ConcurrentQueue<TimeSpan>();


        public DingtalkStreamClientWorker(IServiceProvider serviceProvider , ILogger<DingtalkStreamClientWorker> logger , IOptions<DingtalkStreamOptions> options)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.lazyOptions = options;


            // 预期访问此实例的线程数，以及存储数据的最大容量
            this.activeTaskData = new ConcurrentDictionary<string , MessageEventHanderArgs>();
            this.lazySemaphoreSlim = new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(this.MaxConcurrentTask));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 延迟一小会儿再开始;
            await Task.Delay(1000 , stoppingToken);
            try
            {
                // 创建 Dingtalk Stream Client
                var client = ActivatorUtilities.CreateInstance<DingtalkStreamClient>(serviceProvider);
                client.OnMessage += (sender , messageEventHanderArgs) =>
                {
                    Task.Run(() =>
                    {
                        // 当队列数据超过并发处理上限时，钉钉又进行了重发被接收到时。或者已经在处理的过程中了，则直接忽略
                        if (dataQueue.Any(a => a.Headers.MessageId == messageEventHanderArgs.Headers.MessageId)
                            || activeTaskData.ContainsKey(messageEventHanderArgs.Headers.MessageId))
                        {
                            logger.LogWarning("收到钉钉的重发消息，消息已在本地队列中，本次消息忽略。");
                            return;
                        }

                        // 将新的数据压入到队列中
                        dataQueue.Enqueue(messageEventHanderArgs);
                        // 通知任务开始执行
                        TryNotifyTaskOfPendingWork(stoppingToken);
                        // 收到消息
                        logger.LogInformation($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]收到新的推送数据，当前队列长度：{dataQueue.Count} (+1)");
                    });

                    //Task.Run(async () =>
                    //{
                    //    // 等待信号量
                    //    await SmaphoreSlim.WaitAsync(stoppingToken);
                    //    try
                    //    {
                    //        using var scope = serviceProvider.CreateScope();
                    //        var handler = scope.ServiceProvider.GetRequiredService<IDingtalkStreamMessageHandler>();
                    //        await handler?.HandleMessage(messageEventHanderArgs);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        logger.LogError(ex , "处理消息的过程中发生了错误。");
                    //    }

                    //    // 释放信号量
                    //    SmaphoreSlim.Release();
                    //} , stoppingToken);
                };
                // 启动
                await client.Start();
            }
            catch (Exception ex)
            {
                logger.LogError(ex , "订阅程序已停止运行。");
            }
        }

        void TryNotifyTaskOfPendingWork(CancellationToken stoppingToken)
        {
            // 控制任何时候，都只创建 this.MaxConcurrentTask 个任务去处理队列中的数据
            if (SmaphoreSlim.CurrentCount == 0)
                return;

            // 创建一个新的任务
            Task.Run(async () =>
            {
                // 等待信号量
                await SmaphoreSlim.WaitAsync(stoppingToken);
                // 当队列不空且未达到最大并发数时，开始新任务
                // 如果队列中的数据不为空
                while (!stoppingToken.IsCancellationRequested && !dataQueue.IsEmpty && this.dataQueue.TryPeek(out var _))
                {
                    // 从队列中取出并删除数据，如果队列中没有数据，可能是其他任务把数据取走了
                    if (!dataQueue.TryDequeue(out var data))
                    {
                        continue;
                    }

                    // 加入激活执行数据
                    activeTaskData.TryAdd(data.Headers.MessageId , data);
                    // 创建执行的任务
                    await executeItemAsync(data);
                    // 移除已经完成的任务数据
                    activeTaskData.TryRemove(data.Headers.MessageId , out var _);
                }
                // 释放信号量
                SmaphoreSlim.Release();
            });

            async Task executeItemAsync(MessageEventHanderArgs data)
            {
                Stopwatch stopwatch = new Stopwatch();
                try
                {
                    stoppingToken.ThrowIfCancellationRequested();
                    stopwatch.Start();
                    // 创建一个任务实例所用的作用域，
                    // 这种情况下所使用的所有与作用域有关的服务，都能以作用域的形式呈现使用
                    // 尤其对于数据库操作层，一个任务中只应当产生一个处理实例
                    using var scope = serviceProvider.CreateScope();// 只有当任务结束以后，才去释放它比较合适

                    // 创建一个新的任务实例
                    var handler = scope.ServiceProvider.GetRequiredService<IDingtalkStreamMessageHandler>();
                    // 处理消息
                    await handler?.HandleMessage(data);

                    //var handlerInstance = serviceProvider.GetRequiredService<IRealtimeWorkerHandler<TModel>>();
                    //await handlerInstance.ExecuteAsync(data , stoppingToken);// 开始进行数据上的处理
                }
                catch (OperationCanceledException ex)
                {
                    logger.LogWarning(ex , $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]任务正在取消执行");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex , $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]处理数据时发生异常");
                }
                // 停止计时
                stopwatch.Stop();

                // 将执行时间加入到队列中
                recentExecutionTimes.Enqueue(stopwatch.Elapsed);
                // 如果队列中的数据超过了最大值，则移除最早的数据
                if (recentExecutionTimes.Count > this.Options.RecentExecutionTimeCount)
                {
                    recentExecutionTimes.TryDequeue(out var _);
                }
                // 如果任务执行时间超过了预期值，则记录日志
                if (stopwatch.Elapsed > this.Options.SingleExecuteTimeOut)
                {
                    logger.LogWarning($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]任务执行超过预期值，当前任务执行花费时间：{stopwatch.Elapsed}");
                }
            }
        }
    }
}