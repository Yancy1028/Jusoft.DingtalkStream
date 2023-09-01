using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Robot
{
    public partial class ReceivedRobotMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public class TextContent
        {
            /// <summary>
            /// 文本消息内容。
            /// </summary>

            public string Content { get;internal set; }

        }
    }
}
