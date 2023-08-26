using DingtalkStreamDemo;

using Jusoft.DingtalkStream;





IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDingtalkStream(options =>
        {
            //options.ClientId = "dingXXXXXXXXXXXX";
            //options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            // options.UA = "dingtalk-stream-demo"; // ��չ���Զ����UA
            // options.Subscriptions.Add //  ���ģ�Ҳ��������������

            options.AutoReplySystemMessage = true; // �Զ��ظ� SYSTEM ����Ϣ��ping,disconnect��

        }).RegisterEventSubscription()  // ע���¼����� ����ѡ��
          .RegisterCardInstanceCallback()// ע�ῨƬ�ص� ����ѡ��
          .RegisterIMRobotMessageCallback()// ע���������Ϣ�ص� ����ѡ��
          .AddMessageHandler<DefaultMessageHandler>() //�����Ϣ�������
          .AddHostServices();// ������������������� DingtalkStreamClient

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

//    // options.UA = "dingtalk-stream-demo"; // ��չ���Զ����UA
//    // options.Subscriptions.Add //  ���ģ�Ҳ��������������

//    options.AutoReplySystemMessage = true; // �Զ��ظ� SYSTEM ����Ϣ��ping,disconnect��

//}).RegisterEventSubscription()  // ע���¼����� ����ѡ��
//  .RegisterCardInstanceCallback()// ע�ῨƬ�ص� ����ѡ��
//  .RegisterIMRobotMessageCallback()// ע���������Ϣ�ص� ����ѡ��
//  .AddMessageHandler<DefaultMessageHandler>() //�����Ϣ�������
//  .AddHostServices();// ������������������� DingtalkStreamClient


//var host = builder.Build();

//await host.RunAsync();
