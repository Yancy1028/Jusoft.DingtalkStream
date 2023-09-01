using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Jusoft.DingtalkStream.Core
{
    public class DingtalkStreamException : Exception
    {
        public DingtalkStreamException()
        {
        }

        public DingtalkStreamException(string message) : base(message)
        {
        }

        public DingtalkStreamException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DingtalkStreamException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
