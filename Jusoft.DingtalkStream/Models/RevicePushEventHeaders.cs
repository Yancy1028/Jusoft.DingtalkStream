using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Models
{
    public class RevicePushEventHeaders : RevicePushHeaders
    {
        /// <summary>
        /// 事件类型。
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// 事件的唯一Id。
        /// </summary>
        public string EventId { get; set; }
        /// <summary>
        /// 事件所属的corpId
        /// </summary>
        public string EventCorpId { get; set; }
        /// <summary>
        /// 事件生成时间。
        /// </summary>
        public long EventBornTime { get; set; }
        /// <summary>
        /// 统一应用身份Id。
        /// </summary>
        public string EventUnifiedAppId { get; set; }
    }
}
