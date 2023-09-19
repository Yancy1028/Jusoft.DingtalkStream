using Jusoft.DingtalkStream.Robot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jusoft.DingtalkStream.Core
{
    public static class DingtalkStreamDataPackageExtensions
    {
        /// <summary>
        /// 获取接收到的机器人消息数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ReceivedRobotMessage GetRobotMessageData(this DingtalkStreamDataPackage dataPackage)
        {
            var jsonDoc = JsonDocument.Parse(dataPackage.Data);
            return new ReceivedRobotMessage(jsonDoc.RootElement);
        }
    }
}
