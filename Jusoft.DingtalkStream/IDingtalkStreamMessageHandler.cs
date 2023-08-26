using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream
{
    public interface IDingtalkStreamMessageHandler
    {
        /// <summary>
        /// 收到消息后的处理程序
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        Task HandleMessage(MessageEventHanderArgs e);
    }
}
