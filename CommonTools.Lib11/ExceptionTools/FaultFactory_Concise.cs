using System;
using System.IO;

namespace CommonTools.Lib11.ExceptionTools
{
    public struct Bad
    {
        public static InvalidStateException State<T>(string expectedState, string actualState)
            => Fault.BadState<T>(expectedState, actualState);

        public static InvalidDataException Data(string description)
            => Fault.BadData(description);

        public static ArgumentException Arg(string argumentName, object argumentValue)
            => Fault.BadArg(argumentName, argumentValue);

        public static BadKeyException Key<T>(string expectedKey, string actualKey, string keyDescription = "Key")
            => Fault.BadKey<T>(expectedKey, actualKey, keyDescription);

        public static InvalidCastException Cast<T>(string textToParse, T targetType)
            => Fault.BadCast<T>(textToParse, targetType);

        public static InvalidInsertionException Insert<T>(T forInsertion, string reason)
            => new InvalidInsertionException($"Not allowed to insert ‹{typeof(T).Name}› “{forInsertion}” because {reason}");

        public static InvalidDeletionException Delete<T>(T forDeletion, string reason)
            => new InvalidDeletionException($"Not allowed to delete ‹{typeof(T).Name}› “{forDeletion}” because {reason}");
    }


    public struct Null
    {
        public static NullReferenceException Ref(string variableName) => Fault.NullRef(variableName);
    }


    public struct No
    {
        public static RecordNotFoundException Match<T>(string field, object value)
            => Fault.NoMatch<T>(field, value);
    }


    public struct Missing
    {
        public static FileNotFoundException  File  (string filePath, string fileDescription)  
            => Fault.MissingFile(filePath, fileDescription);

        public static FileNotFoundException  Dir   (string foldrPath) => Fault.MissingDir(foldrPath);
    }
}
