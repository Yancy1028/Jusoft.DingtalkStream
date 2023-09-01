using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Core
{
    /// <summary>
    /// 消息数据事件
    /// </summary>
    public class MessageEventHanderArgs : DingtalkStreamDataPackage
    {
        internal MessageEventHanderArgs(JsonElement payload) : base(payload)
        { }
        /// <summary>
        /// 对服务器进行消息回复，回复消息。请在5s 内进行消息回复的确认，否则服务器将会重发消息。
        /// </summary>
        public Func<byte[], Task> Reply { get; internal set; }
    }
}
