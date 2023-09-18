using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Core
{
    /// <summary>
    /// 消息数据事件
    /// </summary>
    public class MessageEventHanderArgs : DingtalkStreamDataPackage
    {
        private readonly ClientWebSocket clientWebSocket;

        internal MessageEventHanderArgs(JsonElement payload, ClientWebSocket clientWebSocket) : base(payload)
        {
            this.clientWebSocket = clientWebSocket;
        }

        /// <summary>
        /// 对服务器进行消息回复，回复消息。请在5s 内进行消息回复的确认，否则服务器将会重发消息。
        /// </summary>
        public Task Reply(byte[] data)
        {
            return this.clientWebSocket?.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
