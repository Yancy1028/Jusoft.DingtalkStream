using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jusoft.DingtalkStream.Core
{
    public static class DingtalkStreamDataHeadersExtensions
    {
        /// <summary>
        /// 将 <see cref="DingtalkStreamDataHeaders"/> 转换为 <see cref="DingtalkStreamEventDataHeaders"/>
        /// </summary>
        /// <param name="dingtalkStreamDataHeaders">原始的Data Headers</param>
        /// <returns></returns>
        public static DingtalkStreamEventDataHeaders ToEventDataHeaders(this DingtalkStreamDataHeaders dingtalkStreamDataHeaders)
        {
            return new DingtalkStreamEventDataHeaders(dingtalkStreamDataHeaders.Payload);
        }
    }
}
