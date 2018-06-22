using System;

namespace Gooios.Infrastructure.Exceptions
{
    public class ArgumentInvalidException : Exception
    {
        public ArgumentInvalidException() { }

        public ArgumentInvalidException(string message) : base(message)
        { }

        public ArgumentInvalidException(string message, Exception inner) : base(message, inner)
        { }
    }
}
