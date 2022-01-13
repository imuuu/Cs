using System;
using System.Runtime.Serialization;

namespace GameWebApi
{
    public class ExpectationFilt : System.Exception
    {
        public ExpectationFilt()
        {
        }

        public ExpectationFilt(string message) : base(message)
        {
        }

        public ExpectationFilt(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExpectationFilt(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
