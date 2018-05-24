using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Lib11.ExceptionTools
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException(string message) : base(message)
        {
        }
    }
}
