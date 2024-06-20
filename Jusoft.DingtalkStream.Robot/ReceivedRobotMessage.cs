using System.Text.Json;

namespace Jusoft.DingtalkStream.Robot
{
    /// <summary>
    /// 收到机器人消息的数据结构
    /// </summary>
    public partial class ReceivedRobotMessage
    {
        internal ReceivedRobotMessage(JsonElement payload)
        {
            this.Payload = payload;
        }
        /// <summary>
        /// 消息的原始 JSON 数据
        /// </summary>
        public JsonElement Payload { get; }
        /// <summary>
        /// 会话ID。
        /// </summary>
        public string ConversationId => this.Payload.GetProperty("conversationId").GetString();
        /// <summary>
        /// 加密的机器人所在的企业corpId。
        /// </summary>
        public string ChatbotCorpId => this.Payload.GetProperty("chatbotCorpId").GetString();
        /// <summary>
        /// 企业内部群中 @该机器人的成员userid。
        /// </summary>
        public string ChatbotUserId => this.Payload.GetProperty("chatbotUserId").GetString();
        /// <summary>
        /// 加密的消息ID。
        /// </summary>
        public string MsgId => this.Payload.GetProperty("msgId").GetString();
        /// <summary>
        /// 发送者昵称。
        /// </summary>
        public string SenderNick => this.Payload.GetProperty("senderNick").GetString();
        /// <summary>
        /// 是否为管理员。
        /// </summary>
        public bool IsAdmin => this.Payload.GetProperty("isAdmin").GetBoolean();
        /// <summary>
        /// 企业内部群中 @该机器人的成员userid。
        /// </summary>
        public string SenderStaffId => this.Payload.GetProperty("senderStaffId").GetString();
        /// <summary>
        /// 当前会话的Webhook地址过期时间。
        /// </summary>
        public long SessionWebhookExpiredTime => this.Payload.GetProperty("sessionWebhookExpiredTime").GetInt64();
        /// <summary>
        /// 消息的时间戳，单位（毫秒）。
        /// </summary>
        public long CreateAt => this.Payload.GetProperty("createAt").GetInt64();
        /// <summary>
        /// 企业内部群有的发送者当前群的企业corpId。
        /// </summary>
        public string SenderCorpId => this.Payload.GetProperty("senderCorpId").GetString();
        /// <summary>
        /// 1：单聊；2：群聊
        /// </summary>
        public string ConversationType => this.Payload.GetProperty("conversationType").GetString();
        /// <summary>
        /// 加密的发送者ID。<br />
        /// 使用senderStaffId，作为发送者userid值。
        /// </summary>
        public string SenderId => this.Payload.GetProperty("senderId").GetString();
        /// <summary>
        /// 群聊时才有的会话标题。
        /// </summary>
        public string ConversationTitle => this.Payload.GetProperty("conversationTitle").GetString();
        /// <summary>
        /// 是否在@列表中。
        /// </summary>
        public bool IsInAtList
        {
            get
            {
                var jsonElm = this.Payload.GetProperty("isInAtList");
                if (jsonElm.ValueKind == JsonValueKind.Null)
                {
                    return false;
                }
                return jsonElm.GetBoolean();
            }
        }
        /// <summary>
        /// 当前会话的Webhook地址。
        /// </summary>
        public string SessionWebhook => this.Payload.GetProperty("sessionWebhook").GetString();
        public string RobotCode => this.Payload.GetProperty("robotCode").GetString();
        /// <summary>
        /// 消息类型。
        /// </summary>
        public string MsgType => this.Payload.GetProperty("msgtype").GetString();
        /// <summary>
        /// 被@人的信息。
        /// </summary>
        public RobotMessageAtUserCollection AtUsers => new RobotMessageAtUserCollection(this.Payload.GetProperty("atUsers"));

    }
}
