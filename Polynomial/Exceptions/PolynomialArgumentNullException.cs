using System;

namespace PolynomialObject.Exceptions
{
    [Serializable]
    public class PolynomialArgumentNullException : Exception
    {
        public PolynomialArgumentNullException() : base() { }

        public PolynomialArgumentNullException(string message) : base(message) { }

        public PolynomialArgumentNullException(string message, Exception inner) : base(message, inner) { }
    }
}
