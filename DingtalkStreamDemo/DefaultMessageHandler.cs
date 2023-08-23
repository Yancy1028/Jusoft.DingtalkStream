using Jusoft.DingtalkStream;

using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace DingtalkStreamDemo
{
    internal class DefaultMessageHandler : DingtalkStreamMessageHandler
    {
        public override Task HandleMessage(DingtalkStreamClient client, WebSocketMessageType messageType, byte[] message)
        {
            return base.HandleMessage(client, messageType, message);
        }
    }
}
