using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Event
{
    /// <summary>
    /// OA审批实例类型
    /// </summary>
    public enum OAInstanceType
    {
        /// <summary>
        /// 审批实例开始
        /// </summary>
        START,
        /// <summary>
        /// 审批正常结束（同意或拒绝）
        /// </summary>
        FINISH,
        /// <summary>
        /// 审批终止（发起人撤销审批单）
        /// </summary>
        TERMINATE
    }
}
