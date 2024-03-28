using System;

namespace Jusoft.DingtalkStream.Core
{
    public static class DingtalkStreamDataHeadersExtentions
    {
        /// <summary>
        /// EVENT:统一应用身份Id
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetEventUnifiedAppId(this DingtalkStreamDataHeaders headers)
        {
            return headers.Payload.GetProperty("eventUnifiedAppId").ToString();
        }
        /// <summary>
        /// EVENT:事件所属的corpId
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetEventCorpId(this DingtalkStreamDataHeaders headers)
        {
            return headers.Payload.GetProperty("eventCorpId").ToString();
        }
        /// <summary>
        /// EVENT:事件类型
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetEventType(this DingtalkStreamDataHeaders headers)
        {
            return headers.Payload.GetProperty("eventType").ToString();
        }
        /// <summary>
        /// EVENT:事件的唯一Id
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetEventId(this DingtalkStreamDataHeaders headers)
        {
            return headers.Payload.GetProperty("eventId").ToString();
        }
        /// <summary>
        /// EVENT:事件生成时间
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static long GetEventBornTime(this DingtalkStreamDataHeaders headers)
        {
            return Convert.ToInt64(headers.Payload.GetProperty("eventBornTime").ToString());
        }
    }
}
