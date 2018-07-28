using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace CommonTools.Lib45.FileSystemTools
{
    public static class FilePathExtensions
    {
        public static string SHA1ForFile(this string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var algo = new HashLib.Crypto.SHA1();
            //var byts = File.ReadAllBytes(filePath);
            //var byts = ReadAllBytesOrFromCopy(filePath);
            //var byts = ReadAllBytesFromCopy(filePath);
            var byts = ReadAllBytesFromReadOnly(filePath);
            var hash = algo.ComputeBytes(byts);
            return hash.ToString().ToLower();
        }


        private static byte[] ReadAllBytesFromReadOnly(string filePath)
        {
            using (var stream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var readr = new StreamReader(stream))
                {
                    var txt = readr.ReadToEnd();
                    return Encoding.UTF8.GetBytes(txt);
                }
            }
        }


        public static void DeleteIfFound(this string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }


        private static byte[] ReadAllBytesOrFromCopy(string filePath)
        {
            byte[] byts;
            try
            {
                byts = File.ReadAllBytes(filePath);
            }
            catch (IOException)
            {
                byts = ReadAllBytesFromCopy(filePath);
            }
            return byts;
        }

        private static byte[] ReadAllBytesFromCopy(string filePath)
        {
            byte[] byts;
            var tmp = filePath.MakeTempCopy();
            byts = File.ReadAllBytes(tmp);
            File.Delete(tmp);
            return byts;
        }


        public static bool IsInTempDir(this string filePath)
        {
            if (filePath.IsBlank()) return false;
            return filePath.Contains(Path.GetTempPath());
        }


        //public static string CreateTempCopy(this string filePath)
        //{
        //    var tmpPath = Path.GetTempFileName();
        //    File.Copy(filePath, tmpPath, true);
        //    return tmpPath;
        //}
        public static string MakeTempCopy(this string origFilePath, string extension = ".tmp")
        {
            var tmp0 = Path.GetTempFileName();
            File.Delete(tmp0);

            var tmp1 = tmp0 + extension;

            // parallel threads may cause this to be created already
            if (File.Exists(tmp1))
                return origFilePath.MakeTempCopy(extension);

            File.Copy(origFilePath, tmp1, true);
            return tmp1;
        }


        public static string MakeAbsolute(this string filePath)
        {
            if (Path.IsPathRooted(filePath))
                return filePath;
            else
                return Path.Combine(CurrentExe.GetDirectory(), filePath);
        }


        public static string GetVersion(this string filePath)
            => FileVersionInfo.GetVersionInfo(filePath).FileVersion;


        public static string GetShortVersion(this string filePath)
        {
            var ver = filePath.GetVersion();
            if (ver.IsBlank()) return "";
            var ss = ver.Split('.');
            if (ss.Length != 4) return ver;
            return $"{ss[2]}.{int.Parse(ss[3])}";
        }



        public static FileSystemWatcher OnFileChanged(this string filePath, Action onFileChanged)
        {
            if (filePath.IsBlank())
                throw Fault.NullRef("filePath");

            if (!File.Exists(filePath))
                throw Missing.File(filePath, "File to watch");

            var abs    = filePath.MakeAbsolute();
            var dir    = Path.GetDirectoryName(abs);
            var nme    = Path.GetFileName(abs);
            var watchr = new FileSystemWatcher(dir, nme);

            watchr.NotifyFilter = NotifyFilters.LastWrite;
            watchr.Changed += (s, e) =>
            {
                try
                {
                    onFileChanged?.Invoke();
                }
                catch (Exception ex)
                {
                    Alert.Show(ex, "OnFileChanged");
                }
            };
            watchr.EnableRaisingEvents = true;
            return watchr;
        }


        public static bool FileContains(this string filePath, string text)
        {
            if (!File.Exists(filePath))
                throw Missing.File(filePath, $"file with text “{text}”");

            return File.ReadLines(filePath).ToList()
                    .Any(_ => _.Contains(text));
        }


        public static void CreateFileIfMissing(this string filePath)
        {
            if (File.Exists(filePath)) return;
            var abs = filePath.MakeAbsolute();
            var dir = Path.GetDirectoryName(abs);
            Directory.CreateDirectory(dir);
            File.Create(abs);
        }


        //https://stackoverflow.com/a/3822913/3973863
        public static void CopyDirectoryTo(this string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }


        public static string CopyDirectoryToTemp(this string sourcePath)
        {
            var tmpNme = Path.GetTempFileName();
            File.Delete(tmpNme);
            tmpNme = Path.GetFileNameWithoutExtension(tmpNme);

            var tmpDir = Path.Combine(Path.GetTempPath(), tmpNme);
            sourcePath.CopyDirectoryTo(tmpDir);
            return tmpDir;
        }


        //https://stackoverflow.com/a/16216587/3973863
        public static void GrantEveryoneFullControl(this string filePath)
        {
            if (!File.Exists(filePath))
                throw Missing.File(filePath, "Target file for GrantEveryoneFullControl()");

            var info = new FileInfo(filePath);
            var fSec = info.GetAccessControl();
            var user = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            var rule = new FileSystemAccessRule(user,
                        FileSystemRights.FullControl,
                        AccessControlType.Allow);
            fSec.AddAccessRule(rule);
            File.SetAccessControl(filePath, fSec);
        }


        public static bool TryGrantEveryoneFullControl(this string filePath)
        {
            try
            {
                filePath.GrantEveryoneFullControl();
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
