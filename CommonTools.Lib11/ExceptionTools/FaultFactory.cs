using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CommonTools.Lib11.ExceptionTools
{
    public struct Fault
    {
        public static InvalidOperationException CallFirst(string requiredMethod, [CallerMemberName] string callingMethod = null)
            => new InvalidOperationException(
                $"Please call method “{requiredMethod}” before calling “{callingMethod}”.");


        public static InvalidDataException BadData(string description)
            => new InvalidDataException(description);


        public static NullReferenceException NullRef(string variableName)
            => new NullReferenceException(
                $"Variable [{variableName}] should NOT be NULL.");


        public static ArgumentException BadArg(string argumentName, object argumentValue)
            => new ArgumentException(
                $"Invalid [{argumentName}]: “{argumentValue}”");


        public static BadKeyException BadKey<T>(string expectedKey, string actualKey, string keyDescription = "Key")
            => BadKeyException.For<T>(expectedKey, actualKey, keyDescription);


        public static InvalidCastException BadCast<T>(string textToParse, T targetType)
            => new InvalidCastException(
                $"Non-convertible to ‹{typeof(T).Name}›: “{textToParse}”.");


        public static IntrusionAttemptException Intruder()
            => new IntrusionAttemptException();


        public static FileNotFoundException MissingFile(string filePath, string fileDescription)
            => new FileNotFoundException($"Missing “{fileDescription}”: {filePath}");

        public static FileNotFoundException MissingDir(string foldrPath)
            => new FileNotFoundException($"Missing folder: {foldrPath}");


        public static RecordNotFoundException NoMatch<T>(string field, object value)
            => RecordNotFoundException.For<T>(field, value);


        public static InvalidStateException BadState<T>(string expectedState, string actualState)
            => new InvalidStateException($"Expected state of ‹{typeof(T).Name}› to be “{expectedState}”, but was “{actualState}”");
    }
}
