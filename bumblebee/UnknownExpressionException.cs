using System;
using System.Runtime.Serialization;

namespace Bumblebee
{
    /// <summary>
    /// Exception thrown when a snippet can't be successfully parsed.
    /// </summary>
    [Serializable]
    public class UnknownExpressionException : ArgumentException
    {
        /// <summary>
        /// Create a new error with a friendly message.
        /// </summary>
        public UnknownExpressionException(string message)
            : base(message)
        {}
    }
}