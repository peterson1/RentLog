using System;

namespace CommonTools.Lib11.ExceptionTools
{
    public class InvalidDeletionException : Exception
    {
        public InvalidDeletionException(string message) : base(message)
        {
        }


        public static InvalidDeletionException For<T>(T forDeletion, string reason)
            => new InvalidDeletionException($"Not allowed to delete ‹{typeof(T).Name}› “{forDeletion}” because {reason}");
    }
}
