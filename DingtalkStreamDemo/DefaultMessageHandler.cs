using Jusoft.DingtalkStream.Core;
using Jusoft.DingtalkStream.Robot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingtalkStreamDemo
{
    internal class DefaultMessageHandler : IDingtalkStreamMessageHandler
    {
        public async Task HandleMessage(MessageEventHanderArgs e)
        {
            // 此处进行订阅的 Topic 的处理

            var replyMessageData = string.Empty;
            switch (e.Type)
            {
                case SubscriptionType.EVENT:
                    // 事件类型

                    replyMessageData = await DingtalkStreamUtilities.CreateReply_EventSuccess_MessageData("自定义成功消息");
                    // replyMessageData = await DingtalkStreamUtilities.CreateReplyEventFaildMessageData("自定义失败消息");
                    break;
                case SubscriptionType.CALLBACK:

                    //! 需要添加 Jusoft.DingtalkStream.Robot 包
                    // 判断是否机器人回调的消息
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
            }


            // 创建回复的消息
            var replyMessage = await DingtalkStreamUtilities.CreateReplyMessage(e.Headers.MessageId, replyMessageData);
            // 回复消息（注意，请尽早回复消息，避免消息重发的情况）
            await e.Reply(replyMessage);

        }
    }
}
