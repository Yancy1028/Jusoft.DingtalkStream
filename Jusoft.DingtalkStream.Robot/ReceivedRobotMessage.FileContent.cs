using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Robot
{
    public partial class ReceivedRobotMessage
    {
        /// <summary>
        /// 文件消息内容
        /// </summary>
        public class FileContent
        {
            /// <summary>
            /// 文件的下载码，用于换取下载文件的二进制文件
            /// <list type="bullet">
            /// <item>企业内部应用，调用服务端API-下载机器人接收消息的文件内容接口，获取临时下载链接。</item>
            /// <item>第三方企业应用，调用服务端API-下载机器人接收消息的文件内容接口，获取临时下载链接</item>
            /// </list>
            /// </summary>
            public string DownloadCode { get; internal set; }
            /// <summary>
            /// 文件名
            /// </summary>

            public string FileName { get; internal set; }

        }
    }
}
