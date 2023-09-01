using Jusoft.DingtalkStream.Robot;

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Text.Json
{
    /// <summary>
    /// 内部 JsonElement 扩展
    /// </summary>
    internal static class JsonElementExtensions
    {
        /// <summary>
        /// 将json 数据转换为 List<RobotMessageAtUser>
        /// </summary>
        /// <param name="jsonElement"></param>
        /// <returns></returns>
        public static List<RobotMessageAtUser> ToRobotMessageAtUsers(this JsonElement jsonElement)
        {
            var list = new List<RobotMessageAtUser>();
            foreach (var item in jsonElement.EnumerateArray())
            {
                var model = new RobotMessageAtUser();

                foreach (var property in item.EnumerateObject())
                {
                    switch (property.Name)
                    {
                        case "dingtalkId":
                            model.DingtalkId = property.Value.GetString();
                            break;
                        case "staffId":
                            model.StaffId = property.Value.GetString();
                            break;
                        default:
                            break;
                    }
                }
                list.Add(model);
            }
            return list;
        }
    }
}
