using System;

namespace CommonTools.Lib11.ExceptionTools
{
    public class InvalidDeletionException : Exception
    {
        public InvalidDeletionException(string message) : base(message)
        {
        }
    }
}
