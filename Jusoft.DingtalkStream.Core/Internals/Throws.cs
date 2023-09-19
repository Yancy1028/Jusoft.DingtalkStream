using System;
using System.Collections.Generic;
using System.Text;

namespace Jusoft.DingtalkStream.Core.Internals
{
    public static class Throws
    {
        /// <summary>
        /// 触发异常
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="DingtalkStreamException"></exception>
        public static void InternalException(string message)
        {
            throw new DingtalkStreamException(message);
        }
        /// <summary>
        /// 如果参数为null 或空串，则触发异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <exception cref="DingtalkStreamException"></exception>
        public static void IfNullOrWhiteSpace(string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DingtalkStreamException(message);
            }
        }
        /// <summary>
        /// 如果数据为空，则触发异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <exception cref="DingtalkStreamException"></exception>
        public static void IfEmptyArray<T>(IEnumerable<T> value, string message)
        {
            if (value == null || !value.GetEnumerator().MoveNext())
            {
                throw new DingtalkStreamException(message);
            }

        }
    }
}
