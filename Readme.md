# Jusoft.DingtalkStream 使用说明

CSharp SDK for Dingtalk Stream Mode API,Compared with the webhook mode, it is easier to access the DingTalk chatbot

C# 版本的钉钉Stream模式API SDK，支持订阅内容扩展，目前有【事件推送】【机器人消息回调】【卡片回调】


## 使用说明

[直接去看代码示例](#代码示例)

### 准备工作
- 钉钉开发者账号，具备创建企业内部应用的权限，详见[成为钉钉开发者](https://open.dingtalk.com/document/orgapp/become-a-dingtalk-developer)
- 支持 .NET Core3.1、.NET 6.0、.NET Standard 2.1 的任意开发环境

### 安装

>在Visual Studio 中“工具”——“Nuget包管理器”——"程序包管理器控制台" 执行
>
```bash

// 直接安装 Jusoft.DingtalkStream 包，会自动安装 Jusoft.DingtalkStream.Core 和 Jusoft.DingtalkStream.Robot
Install-Package Jusoft.DingtalkStream // 集合了所有的回调处理能力


Install-Package Jusoft.DingtalkStream.Core // DingStream 的核心处理能力，可以独立使用
Install-Package Jusoft.DingtalkStream.Robot // 实现了针对机器人回调的处理能力以及辅助方法

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
    public async Task HandleMessage(MessageEventHanderArgs e)
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
                 
                    // 判断是否机器人回调的消息
                    //! 需要添加 Jusoft.DingtalkStream.Robot 包
                    if (e.Headers.IsRobotTopic())
                    {
                        var robotMessage = e.GetRobotMessageData();

                        // 通过消息类型 robotMessage.MsgType 来识别具体的消息内容

                        // 获取语音消息内容
                        //var content=robotMessage.GetAudioContent();
                        // 获取富文件消息内容
                        //var content=robotMessage.GetFileContent();
                        // 获取富图片消息内容
                        //var content=robotMessage.GetPictureContent();
                        // 获取富文本消息内容
                        //var content =robotMessage.GetRichTextContent();
                        // 获取文本消息内容
                        //var content = robotMessage.GetTextContent();
                        // 获取视频消息内容
                        //var content=robotMessage.GetVideoContent();

                        // 使用机器人发送 文本 消息
                        await DingtalkRobotWebhookUtilites.SendTextMessage(robotMessage.SessionWebhook, "@43475226895352吃饭了吗？", atUserIds: new string[] { "43475226895352" });
                        // 使用机器人发送 Link 消息
                        await DingtalkRobotWebhookUtilites.SendLinkMessage(robotMessage.SessionWebhook, "这是Link消息", "这是一个Link消息", "https://img.alicdn.com/tfs/TB1NwmBEL9TBuNjy1zbXXXpepXa-2400-1218.png", "https://open.dingtalk.com/document/");
                        // 使用机器人发送 Markdown 消息
                        await DingtalkRobotWebhookUtilites.SendMarkdownMessage(robotMessage.SessionWebhook, "杭州天气", "#### 杭州天气 @43475226895352 \n> 9度，西北风1级，空气良89，相对温度73%\n> ![screenshot](https://img.alicdn.com/tfs/TB1NwmBEL9TBuNjy1zbXXXpepXa-2400-1218.png)\n> ###### 10点20分发布 [天气](https://www.dingalk.com) \n", atUserIds: new string[] { "43475226895352" });
                        // 使用机器人发送 ActionCard 消息
                        await DingtalkRobotWebhookUtilites.SendActionCardMessage(robotMessage.SessionWebhook, "乔布斯 20 年前想打造一间苹果咖啡厅，而它正是 Apple Store 的前身",
                                                                                                   "![screenshot](https://img.alicdn.com/tfs/TB1NwmBEL9TBuNjy1zbXXXpepXa-2400-1218.png) \n\n #### 乔布斯 20 年前想打造的苹果咖啡厅 \n\n Apple Store 的设计正从原来满满的科技感走向生活化，而其生活化的走向其实可以追溯到 20 年前苹果一个建立咖啡馆的计划",
                                                                                                   BtnOrientation.Vertical,
                                                                                                   ("内容不错", "https://www.dingtalk.com/"),
                                                                                                   ("不感兴趣", "https://www.dingtalk.com/"),
                                                                                                   ("🥩", "https://www.dingtalk.com/"),
                                                                                                   ("ヽ(●-`Д´-)ノ", "https://www.dingtalk.com/"),
                                                                                                   ("ヾﾉ≧∀≦)o死开!", "https://www.dingtalk.com/"),
                                                                                                   ("ヾ(≧O≦)〃嗷~", "https://www.dingtalk.com/"),
                                                                                                   ("ლ(╹◡╹ლ)", "https://www.dingtalk.com/"),
                                                                                                   ("┣▇▇▇═─ ", "https://www.dingtalk.com/"),
                                                                                                   ("୧(๑•̀⌄•́๑)૭碉堡了", "https://www.dingtalk.com/"),
                                                                                                   ("(@﹏@)~", "https://www.dingtalk.com/")
                                                                                               );

                        // 使用机器人发送 FeedCard 消息
                        await DingtalkRobotWebhookUtilites.SendFeedCardMessage(robotMessage.SessionWebhook,
                                                ("时代的火车向前开1", "https://www.dingtalk.com/", "https://img.alicdn.com/tfs/TB1NwmBEL9TBuNjy1zbXXXpepXa-2400-1218.png"),
                                                ("时代的火车向前开2", "https://www.dingtalk.com/", "https://img.alicdn.com/tfs/TB1NwmBEL9TBuNjy1zbXXXpepXa-2400-1218.png")
                                                                        );
                    }
                    else
                    {
                        // 处理非机器人消息
                    }
                    replyMessageData = DingtalkStreamUtilities.CreateReply_Callback_MessageData("自定义回调结果");

                    break;
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
          .RegisterIMRobotMessageCallback()// 注册机器人消息回调 （可选）需要安装：Jusoft.DingtalkStream.Robot
           //.RegisterSubscription("{type}","{topic}")// 注册订阅基础方法
          .AddMessageHandler<DefaultMessageHandler>() //添加消息处理服务
          .AddHostServices();// 添加主机服务，用于启动 DingtalkStreamClient

    })
    .Build();

await host.RunAsync();

```
## 技术支持

[点击链接，加入Stream模式共创群交流](https://open-dingtalk.github.io/developerpedia/docs/explore/support/?via=moon-group)