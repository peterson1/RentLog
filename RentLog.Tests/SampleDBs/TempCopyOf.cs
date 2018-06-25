using CommonTools.Lib45.FileSystemTools;
using System.IO;

namespace RentLog.Tests.SampleDBs
{
    internal class TempCopyOf
    {
        internal static SampleArgs Jun17_Balanced(string roles)
        {
            var origDB  = SampleArgs.FindDB(SampleDir.JUN17_BALANCED);
            var srcDir  = Path.GetDirectoryName(origDB);
            var destDir = srcDir.CopyDirectoryToTemp();
            var origDir = SampleArgs.DirPath;

            SampleArgs.DirPath = destDir;
            SampleArgs.DirName = "";

            var args = new SampleArgs(roles);
            SampleArgs.DirPath = origDir;
            return args;
        }
    }
}
