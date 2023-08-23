using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Models
{
    /// <summary>
    /// 接收到的推送的headers 信息
    /// </summary>
    public class RevicePushHeaders
    {
        /// <summary>
        /// 本次推送的业务 Topic
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 推送消息ID，标记一次推送，客户端需要关注此信息并且在响应的时候将此信息回传给服务端
        /// </summary>
        public string MessageId { get; set; }
        /// <summary>
        /// 标记推送数据的格式，默认为application/json，代表推送的数据为一个json字符串；该字段预留将来支持二进制数据；
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 推送时间,为unix时间戳，单位：毫秒
        /// </summary>
        public long Time { get; set; }

    }
}
