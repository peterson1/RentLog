using CommonTools.Lib45.FileSystemTools;
using System.IO;

namespace RentLog.Tests.SampleDBs
{
    internal class TempCopyOf
    {
        internal static SampleArgs Jun17_Balanced()
        {
            var origDB  = SampleArgs.FindDB(SampleDir.JUN17_BALANCED);
            var srcDir  = Path.GetDirectoryName(origDB);
            var destDir = srcDir.CopyDirectoryToTemp();
            var origDir = SampleArgs.DirPath;

            SampleArgs.DirPath = destDir;
            SampleArgs.DirName = "";

            var args = new SampleArgs();
            SampleArgs.DirPath = origDir;
            return args;
        }
    }
}
