using System;

namespace CommonTools.Lib11.ExceptionTools
{
    public class BadKeyException : Exception
    {
        public BadKeyException(string message) : base(message)
        {
        }


        public static BadKeyException For<T>(string expectedKey, string actualKey, string keyDescription = "Key")
            => new BadKeyException(
                $"Expected ‹{typeof(T).Name}› {keyDescription} to be “{expectedKey}” but was “{actualKey}”.");
    }
}
