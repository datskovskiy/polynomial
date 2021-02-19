using System;
using System.Runtime.Serialization;

namespace PolynomialObject.Exceptions
{
    [Serializable]
    public class PolynomialArgumentNullException : Exception
    {
        public PolynomialArgumentNullException() : base() { }

        public PolynomialArgumentNullException(string message) : base(message) { }

        public PolynomialArgumentNullException(string message, Exception inner) : base(message, inner) { }

        protected PolynomialArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
