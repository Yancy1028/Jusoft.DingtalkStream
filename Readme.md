# Jusoft.DingtalkStream 使用说明

CSharp SDK for Dingtalk Stream Mode API,Compared with the webhook mode, it is easier to access the DingTalk chatbot

C# 版本的钉钉Stream模式API SDK，支持订阅内容扩展，目前有【事件推送】【机器人消息回调】【卡片回调】


## 使用说明

### 准备工作
- 钉钉开发者账号，具备创建企业内部应用的权限，详见[成为钉钉开发者](https://open.dingtalk.com/document/orgapp/become-a-dingtalk-developer)
- 支持.NET Core3.1、.NET 6.0、.NET Standard 2.1 的任意开发环境

### 安装
```bash
Install-Package Jusoft.DingtalkStream
```

### 快速开始指南
> 注意：消息接收模式中，选择 “Stream 模式”
> ![Stream 模式](https://img.alicdn.com/imgextra/i3/O1CN01XL4piO1lkYX2F6sW6_!!6000000004857-0-tps-896-522.jpg)
> 点击“点击调试”按钮，可以创建测试群进行测试。

- 1、创建企业内部应用
  - 进入[钉钉开发者后台](https://open-dev.dingtalk.com/)，创建企业内部应用，获取ClientID（即 AppKey）和ClientSecret（ 即AppSecret）。
  - 发布应用：在开发者后台左侧导航中，点击“版本管理与发布”，点击“确认发布”，并在接下来的可见范围设置中，选择“全部员工”，或者按需选择部分员工。

- 2、Stream 模式的机器人（可选）
  - 如果不需要使用机器人功能的话，可以不用创建。
  - 在应用管理的左侧导航中，选择“消息推送”，打开机器人能力，设置机器人基本信息。

### 事件订阅切换到 Stream 模式（可选）

进入钉钉开发者后台，选择企业内部应用，在应用管理的左侧导航中，选择“事件与回调”。
“订阅管理”中，“推送方式”选项中，选择 “Stream 模式”，并保存

## 代码示例

### 方式1、常规方式使用

```csharp
// =================  Program.cs  ====================

// 设置
var options=new DingtalkStreamOptions(){
    // 三方应用使用: SuiteKey
    // 企业自建使用：Appkey
    ClientId="dingXXXXXXXXXXXX",
    // 三方应用使用：SuiteSecret
    // 企业自建使用：AppSecret
    ClientSecret="xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
};
// 创建Stearm回调处理实例
var client=new DingtalkStreamClient();

// 注册事件回调
client.RegisterEventSubscription()；

// 启动（注意：异步）
client.Start();
```

### 方式2、通过注入方式使用

> 请参考 DingtalkStreamDemo 中的代码示例

```csharp
// =================  Program.cs  ====================

// 注册后可使用 DingtalkStreamClient （单例）进行使用
builder.Services.AddDingtalkStream(options =>
{
    // 三方应用使用: SuiteKey
    // 企业自建使用：Appkey
    options.ClientId = "dingXXXXXXXXXXXX";
    // 三方应用使用：SuiteSecret
    // 企业自建使用：AppSecret
    options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
});

// 通过实现 DingtalkStreamMessageHandler 的单例可处理所有能收到的消息
// 其中 DingtalkStreamMessageHandler 内部已实现 type:SYSTEM,topic:ping 及 type:SYSTEM,topic:disconnect 回调的处理
```csharp

```

```csharp
// =================  Worker.cs  ====================

// Worker 中获得 DingtalkStreamClient client 的注册服务

// 在执行前进行注册
// 注册事件回调
client.RegisterEventSubscription()；

// 注册完毕后执启动（异步）
client.Start();
```

## 技术支持

[点击链接，加入Stream模式共创群交流](https://open-dingtalk.github.io/developerpedia/docs/explore/support/?via=moon-group)