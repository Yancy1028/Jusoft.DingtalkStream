
namespace Jusoft.DingtalkStream.Robot
{
    public class RobotMessageAtUser
    {
        /// <summary>
        /// 加密的发送者ID。
        /// </summary>
        public string DingtalkId { get; set; }
        /// <summary>
        /// 企业内部群有的发送者在企业内的userid。
        /// </summary>
        public string StaffId { get; set; }
    }

}
