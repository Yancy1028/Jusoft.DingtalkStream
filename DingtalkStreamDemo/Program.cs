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

            // options.UA = "dingtalk-stream-demo"; // 扩展的自定义的UA
            // options.Subscriptions.Add //  订阅，也可以在这里配置

            options.AutoReplySystemMessage = true; // 自动回复 SYSTEM 的消息（ping,disconnect）

            options.OnStarted = (client) =>
            {
                Console.WriteLine("订阅程序已启动。");
            };
            options.OnStoped = (client , ex) =>
            {
                // ex : 停止的异常原因
                Console.WriteLine("订阅程序已停止运行。");
            };


        })
          //.RegisterEventSubscription()  // 注册事件订阅 （可选）
          //.RegisterCardInstanceCallback()// 注册卡片回调 （可选）
          .RegisterIMRobotMessageCallback()// 注册机器人消息回调 （可选） // 需要添加 Jusoft.DingtalkStream.Robot 包
          .AddMessageHandler<DefaultMessageHandler>() //添加消息处理服务
          .AddHostServices();// 添加主机服务，用于启动 DingtalkStreamClient

    })
    .Build();

await host.RunAsync();
