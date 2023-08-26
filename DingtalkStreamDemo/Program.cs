using DingtalkStreamDemo;

using Jusoft.DingtalkStream;





IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDingtalkStream(options =>
        {
            //options.ClientId = "dingXXXXXXXXXXXX";
            //options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            // options.UA = "dingtalk-stream-demo"; // 扩展的自定义的UA
            // options.Subscriptions.Add //  订阅，也可以在这里配置

            options.AutoReplySystemMessage = true; // 自动回复 SYSTEM 的消息（ping,disconnect）

        }).RegisterEventSubscription()  // 注册事件订阅 （可选）
          .RegisterCardInstanceCallback()// 注册卡片回调 （可选）
          .RegisterIMRobotMessageCallback()// 注册机器人消息回调 （可选）
          .AddMessageHandler<DefaultMessageHandler>() //添加消息处理服务
          .AddHostServices();// 添加主机服务，用于启动 DingtalkStreamClient

    })
    .Build();

await host.RunAsync();


//var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddDingtalkStream(options =>
//{
//    //options.ClientId = "dingXXXXXXXXXXXX";
//    //options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
//    options.ClientId = "dingkwejbpileuhzvjbb";
//    options.ClientSecret = "S7cuW_1eWffRnKWcsNIi1fvypnx28Q2yUodtGVXkV2hCYEySNvLPPUBu-_PwiarH";

//    // options.UA = "dingtalk-stream-demo"; // 扩展的自定义的UA
//    // options.Subscriptions.Add //  订阅，也可以在这里配置

//    options.AutoReplySystemMessage = true; // 自动回复 SYSTEM 的消息（ping,disconnect）

//}).RegisterEventSubscription()  // 注册事件订阅 （可选）
//  .RegisterCardInstanceCallback()// 注册卡片回调 （可选）
//  .RegisterIMRobotMessageCallback()// 注册机器人消息回调 （可选）
//  .AddMessageHandler<DefaultMessageHandler>() //添加消息处理服务
//  .AddHostServices();// 添加主机服务，用于启动 DingtalkStreamClient


//var host = builder.Build();

//await host.RunAsync();
