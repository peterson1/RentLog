using CommonTools.Lib11.StringTools;
using System;

namespace CommonTools.Lib11.ExceptionTools
{
    public class LockedFileException : Exception
    {
        public LockedFileException()
        {
        }

        public LockedFileException(string filePath) : base(ComposeMessage(filePath))
        {
        }

        private static string ComposeMessage(string filePath)
            => $"The file is locked: {L.f}{filePath}";
    }
}
