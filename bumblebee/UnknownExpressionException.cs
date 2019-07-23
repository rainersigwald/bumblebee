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
    }
}