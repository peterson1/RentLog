using System;

namespace CommonTools.Lib11.ExceptionTools
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        {
        }

        private RecordNotFoundException(string message) : base(message)
        {
        }

        public static RecordNotFoundException For<T>(string field, object value)
            => new RecordNotFoundException(
                $"‹{typeof(T).Name}› not found whose “{field}” is [{value}].");
    }
}
