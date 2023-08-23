# 使用说明

## 安装


## 代码示例

### 方式1、常规方式使用

```csharp
    
```

### 方式2、通过注入方式使用

> 请参考 DingtalkStreamDemo 中的代码示例

```csharp
// Program.cs 添加注册服务的代码
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

// 通过注册
```

```csharp
// Worker 中获得 DingtalkStreamClient client 的注册服务

// 在执行前进行注册
// 注册事件回调
client.RegisterEventSubscription()；

// 注册完毕后执启动
client.Start();
```

