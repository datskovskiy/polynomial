using System;
using System.Runtime.Serialization;

namespace PolynomialObject.Exceptions
{
    [Serializable]
    public class PolynomialArgumentException : Exception
    {

        public PolynomialArgumentException() : base() { }

        public PolynomialArgumentException(string message) : base(message) { }

        public PolynomialArgumentException(string message, Exception inner) : base(message, inner) { }

        protected PolynomialArgumentException(SerializationInfo info, StreamingContext context) : base(info, context) { }


    }
}
