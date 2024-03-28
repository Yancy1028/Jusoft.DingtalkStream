using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream.Event
{
    public class ReceivedEventMessage
    {
        internal ReceivedEventMessage(JsonElement payload)
        {
            this.Payload = payload;
        }
        /// <summary>
        /// 消息的原始 JSON 数据
        /// </summary>
        public JsonElement Payload { get; }
        /// <summary>
    }
}
