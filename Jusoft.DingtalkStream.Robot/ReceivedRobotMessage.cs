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
        public string ConversationId => this.Payload.TryGetProperty("conversationId", out var conversationId) ? conversationId.GetString() : string.Empty;
        /// <summary>
        /// 加密的机器人所在的企业corpId。
        /// </summary>
        public string ChatbotCorpId => this.Payload.TryGetProperty("chatbotCorpId", out var chatbotCorpId) ? chatbotCorpId.GetString() : string.Empty;
        /// <summary>
        /// 企业内部群中 @该机器人的成员userid。
        /// </summary>
        public string ChatbotUserId => this.Payload.TryGetProperty("chatbotUserId", out var chatbotUserId) ? chatbotUserId.GetString() : string.Empty;
        /// <summary>
        /// 加密的消息ID。
        /// </summary>
        public string MsgId => this.Payload.TryGetProperty("msgId", out var msgId) ? msgId.GetString() : string.Empty;
        /// <summary>
        /// 发送者昵称。
        /// </summary>
        public string SenderNick => this.Payload.TryGetProperty("senderNick", out var senderNick) ? senderNick.GetString() : string.Empty;
        /// <summary>
        /// 是否为管理员。
        /// </summary>
        public bool IsAdmin => this.Payload.TryGetProperty("isAdmin", out var isAdmin) ? isAdmin.GetBoolean() : false;
        /// <summary>
        /// 企业内部群中 @该机器人的成员userid。
        /// </summary>
        public string SenderStaffId => this.Payload.TryGetProperty("senderStaffId", out var senderStaffId) ? senderStaffId.GetString() : string.Empty;
        /// <summary>
        /// 当前会话的Webhook地址过期时间。
        /// </summary>
        public long SessionWebhookExpiredTime => this.Payload.TryGetProperty("sessionWebhookExpiredTime", out var sessionWebhookExpiredTime) ? sessionWebhookExpiredTime.GetInt64() : 0;
        /// <summary>
        /// 消息的时间戳，单位（毫秒）。
        /// </summary>
        public long CreateAt => this.Payload.TryGetProperty("createAt", out var createAt) ? createAt.GetInt64() : 0;
        /// <summary>
        /// 企业内部群有的发送者当前群的企业corpId。
        /// </summary>
        public string SenderCorpId => this.Payload.TryGetProperty("senderCorpId", out var senderCorpId) ? senderCorpId.GetString() : string.Empty;
        /// <summary>
        /// 1：单聊；2：群聊
        /// </summary>
        public string ConversationType => this.Payload.TryGetProperty("conversationType", out var conversationType) ? conversationType.GetString() : string.Empty;
        /// <summary>
        /// 加密的发送者ID。<br />
        /// 使用senderStaffId，作为发送者userid值。
        /// </summary>
        public string SenderId => this.Payload.TryGetProperty("senderId", out var senderId) ? senderId.GetString() : string.Empty;
        /// <summary>
        /// 群聊时才有的会话标题。
        /// </summary>
        public string ConversationTitle => this.Payload.TryGetProperty("conversationTitle", out var conversationTitle) ? conversationTitle.GetString() : string.Empty;
        /// <summary>
        /// 是否在@列表中。
        /// </summary>
        public bool IsInAtList => this.Payload.TryGetProperty("isInAtList", out var isInAtList) ? isInAtList.GetBoolean() : false;
        /// <summary>
        /// 当前会话的Webhook地址。
        /// </summary>
        public string SessionWebhook => this.Payload.TryGetProperty("sessionWebhook", out var sessionWebhook) ? sessionWebhook.GetString() : string.Empty;
        public string RobotCode => this.Payload.TryGetProperty("robotCode", out var robotCode) ? robotCode.GetString() : string.Empty;
        /// <summary>
        /// 消息类型。
        /// </summary>
        public string MsgType => this.Payload.TryGetProperty("msgtype", out var msgType) ? msgType.GetString() : string.Empty;
        /// <summary>
        /// 被@人的信息。
        /// </summary>
        public RobotMessageAtUserCollection AtUsers => this.Payload.TryGetProperty("atUsers", out var atUsers) ? new RobotMessageAtUserCollection(atUsers) : null;

    }
}
