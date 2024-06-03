using DingtalkStreamDemo;

using Jusoft.DingtalkStream.Core;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context , services) =>
    {
        services.AddDingtalkStream(options =>
        {
            //options.ClientId = "dingXXXXXXXXXXXXXXXXXX";
            //options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            // appsettings.json 上配置
            options.ClientId = context.Configuration["ClientId"];
            options.ClientSecret = context.Configuration["ClientSecret"];

            options.AutoReplySystemMessage = true; // 自动回复 SYSTEM 的消息（ping,disconnect）

            // options.UA = "dingtalk-stream-demo"; // 扩展的自定义的UA

            //options.MaxQueueCount = 1000; // 队列最大长度
            //options.MaxTaskCount = Environment.ProcessorCount; // 最大并行任务数量，默认取CPU 核心数
            //options.RecentExecutionTimeCount = 100; // 观察的任务执行时间的参考最近执行的任务数
            //options.TimeInterval = TimeSpan.FromMinutes(5); // 单次检测新数据的时间
            //options.SingleExecuteTimeOut = TimeSpan.FromMinutes(5); // 单个任务执行超时时间

            options.OnStarted = (client) =>
            {
                Console.WriteLine("订阅程序已启动。");
            };
            options.OnStoped = (client , ex) =>
            {
                // ex : 停止的异常原因
                Console.WriteLine("订阅程序已停止运行。");
            };

            // options.Subscriptions.Add //  订阅，也可以在这里配置
        })
          .RegisterEventSubscription()  // 注册事件订阅 （可选）
          .RegisterCardInstanceCallback()// 注册卡片回调 （可选）
          .RegisterIMRobotMessageCallback()// 注册机器人消息回调 （可选） // 需要添加 Jusoft.DingtalkStream.Robot 包
          .AddMessageHandler<DefaultMessageHandler>() //添加消息处理服务
          .AddHostServices();// 添加主机服务，用于启动 DingtalkStreamClient

    })
    .Build();

await host.RunAsync();
