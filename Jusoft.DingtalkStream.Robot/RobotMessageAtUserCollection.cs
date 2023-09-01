using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace Jusoft.DingtalkStream.Robot
{
    /// <summary>
    /// 机器人消息中At 用户的信息
    /// </summary>
    public class RobotMessageAtUserCollection : ReadOnlyCollection<RobotMessageAtUser>
    {
        internal RobotMessageAtUserCollection(JsonElement payload) : base(payload.ToRobotMessageAtUsers())
        {
            this.Payload = payload;
        }
        /// <summary>
        /// 消息的原始 JSON 数据
        /// </summary>
        public JsonElement Payload { get; }

    }
}
