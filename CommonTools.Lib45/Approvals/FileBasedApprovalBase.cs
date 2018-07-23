using CommonTools.Lib11.Approvals;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using System.IO;

namespace CommonTools.Lib45.Approvals
{
    public class FileBasedApprovalBase
    {
        private const string FNAME_FMT = "Request_{0}.env";
        private string _dir;


        public FileBasedApprovalBase(string folderPath, string userName)
        {
            _dir     = folderPath;
            Username = userName;
        }


        public string Username { get; }



        public bool TryGetEnvelope<T>(string requestKey, out ApprovalEnvelope<T> requestEnvelope)
            => TryGetEnvelope(out requestEnvelope, GetFilePath(requestKey));


        public bool TryGetEnvelope<T>(out ApprovalEnvelope<T> requestEnvelope, string filePath)
        {
            requestEnvelope = null;
            if (!File.Exists(filePath)) return false;

            requestEnvelope = JsonFile.Read<ApprovalEnvelope<T>>(filePath);
            return true;
        }


        protected void WriteEnvelope<T>(ApprovalEnvelope<T> envelope)
        {
            var fPath = GetFilePath(envelope.RequestKey);
            if (File.Exists(fPath)) File.Delete(fPath);
            JsonFile.Write(envelope, fPath, true);
        }


        private static string EnsureExists(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }


        protected string GetFilePath(string requestKey)
            => Path.Combine(EnsureExists(_dir), GetFileName(requestKey));


        private static string GetFileName(string requestKey)
            => string.Format(FNAME_FMT, requestKey.SHA1ForUTF8());
    }
}
