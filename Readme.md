# Jusoft.DingtalkStream 使用说明

CSharp SDK for Dingtalk Stream Mode API,Compared with the webhook mode, it is easier to access the DingTalk chatbot

C# 版本的钉钉Stream模式API SDK，支持订阅内容扩展，目前有【事件推送】【机器人消息回调】【卡片回调】


## 使用说明

[直接去看代码示例](#代码示例)

### 准备工作
- 钉钉开发者账号，具备创建企业内部应用的权限，详见[成为钉钉开发者](https://open.dingtalk.com/document/orgapp/become-a-dingtalk-developer)
- 支持 .NET Core3.1、.NET 6.0、.NET Standard 2.1 的任意开发环境

### 安装
```bash
Install-Package Jusoft.DingtalkStream
```

### 快速开始指南
> 注意：消息接收模式中，选择 “Stream 模式”
> ![Stream 模式](https://img.alicdn.com/imgextra/i3/O1CN01XL4piO1lkYX2F6sW6_!!6000000004857-0-tps-896-522.jpg)
> 点击“点击调试”按钮，可以创建测试群进行测试。

- **1、创建企业内部应用**
  - 进入[钉钉开发者后台](https://open-dev.dingtalk.com/)，创建企业内部应用，获取ClientID（即 AppKey）和ClientSecret（ 即AppSecret）。
  - 发布应用：在开发者后台左侧导航中，点击“版本管理与发布”，点击“确认发布”，并在接下来的可见范围设置中，选择“全部员工”，或者按需选择部分员工。

- **2、Stream 模式的机器人（可选）**
  - 如果不需要使用机器人功能的话，可以不用创建。
  - 在应用管理的左侧导航中，选择“消息推送”，打开机器人能力，设置机器人基本信息。

### 事件订阅切换到 Stream 模式（可选）

进入钉钉开发者后台，选择企业内部应用，在应用管理的左侧导航中，选择“事件与回调”。
“订阅管理”中，“推送方式”选项中，选择 “Stream 模式”，并保存

## 代码示例

> **创建项目类型推荐选择：辅助角色服务(Worker Service)**

```csharp
// =================  DefaultMessageHandler.cs  ====================
// 实现消息处理类
//
// 继承 IDingtalkStreamMessageHandler
// 重写 HandleMessage 方法，可处理所有能收到的消息
public class DefaultMessageHandler : IDingtalkStreamMessageHandler
{
    public override async Task HandleMessage(MessageEventHanderArgs e)
    {
        // 此处进行订阅的 Topic 的处理,处理消息的代码

        var replyMessageData = string.Empty;// 记录最终回复消息的Data 的内容
        switch (e.Type)
        {
            case SubscriptionType.EVENT:
                // 事件推送的处理
                replyMessageData = await DingtalkStreamUtilities.CreateReplyEventSuccessMessageData("自定义成功消息");
                // replyMessageData = await DingtalkStreamUtilities.CreateReplyEventFaildMessageData("自定义失败消息");
                break;
            case SubscriptionType.CALLBACK:
                // 回调推送的处理
                replyMessageData = await DingtalkStreamUtilities.CreateReplyCallbackMessageData("自定义回调结果");
                break;
        }

        // 创建回复的消息
        var replyMessage = await DingtalkStreamUtilities.CreateReplyMessage(e.Headers.MessageId, replyMessageData);
        // 回复消息方法
        await e.Reply(replyMessage);

        //return Task.CompletedTask;
    }
}
```

```csharp
// =================  Program.cs  ====================
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDingtalkStream(options =>
        {
            options.ClientId = "dingXXXXXXXXXXXX";
            options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            // options.UA = "dingtalk-stream-demo/1.0.0"; // 扩展的自定义的UA
            // options.Subscriptions.Add //  订阅，也可以在这里配置

            options.AutoReplySystemMessage = true; // 自动回复 SYSTEM 的消息（ping,disconnect）

        }).RegisterEventSubscription()  // 注册事件订阅 （可选）
          .RegisterCardInstanceCallback()// 注册卡片回调 （可选）
          .RegisterIMRobotMessageCallback()// 注册机器人消息回调 （可选）
           //.RegisterSubscription("{type}","{topic}")// 注册订阅基础方法
          .AddMessageHandler<DefaultMessageHandler>() //添加消息处理服务
          .AddHostServices();// 添加主机服务，用于启动 DingtalkStreamClient

    })
    .Build();

await host.RunAsync();

```
## 技术支持

[点击链接，加入Stream模式共创群交流](https://open-dingtalk.github.io/developerpedia/docs/explore/support/?via=moon-group)