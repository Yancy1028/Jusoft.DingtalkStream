using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Event
{
    public enum OATaskType
    {
        /// <summary>
        /// 审批任务开始
        /// </summary>
        START,
        /// <summary>
        /// 审批任务正常结束（完成或转交）
        /// </summary>
        FINISH,
        /// <summary>
        /// 说明当前节点有多个审批人并且是或签，其中一个人执行了审批，其他审批人会推送cancel类型事件
        /// </summary>
        CANCEL
    }
}
