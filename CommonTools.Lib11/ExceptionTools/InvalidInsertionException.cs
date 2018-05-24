using System;

namespace CommonTools.Lib11.ExceptionTools
{
    public class InvalidInsertionException : Exception
    {
        public InvalidInsertionException(string message) : base(message)
        {
        }
    }
}
