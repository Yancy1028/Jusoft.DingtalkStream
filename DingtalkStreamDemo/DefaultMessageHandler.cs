using Jusoft.DingtalkStream;

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


                    replyMessageData = await DingtalkStreamUtilities.CreateReplyEventSuccessMessageData("自定义成功消息");
                    // replyMessageData = await DingtalkStreamUtilities.CreateReplyEventFaildMessageData("自定义失败消息");
                    break;
                case SubscriptionType.CALLBACK:

                    replyMessageData = await DingtalkStreamUtilities.CreateReplyCallbackMessageData("自定义回调结果");
                    break;
            }


            // 创建回复的消息
            var replyMessage = await DingtalkStreamUtilities.CreateReplyMessage(e.Headers.MessageId, replyMessageData);
            // 回复消息
            await e.Reply(replyMessage);

            //return Task.CompletedTask;
        }
    }
}
