using System;
using System.Runtime.Serialization;

namespace VirtualAssistantBusinessLogic.SparQL
{
    [Serializable]
    public class CountZeroException : Exception
    {
        public CountZeroException()
        {
        }

        public CountZeroException(string message) : base(message)
        {
        }

        public CountZeroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CountZeroException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}