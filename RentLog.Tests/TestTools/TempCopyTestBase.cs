using CommonTools.Lib45.FileSystemTools;
using RentLog.Tests.SampleDBs;
using System;
using System.IO;

namespace RentLog.Tests.TestTools
{
    public abstract class TempCopyTestBase : IDisposable
    {
        public TempCopyTestBase()
        {
            var srcDir = GetSampleDBsDir();
            TempDir = srcDir.CopyDirectoryToTemp();
        }


        public string TempDir { get; }

        protected abstract string SampleDirName { get; }


        protected SampleArgs GetTempSampleArgs()
        {
            var origDir = SampleArgs.DirPath;

            SampleArgs.DirPath = TempDir;
            SampleArgs.DirName = "";

            var args = new SampleArgs();
            SampleArgs.DirPath = origDir;
            return args;
        }


        private string GetSampleDBsDir()
        {
            var origDB = SampleArgs.FindDB(SampleDirName);
            return Path.GetDirectoryName(origDB);
        }


        public void Dispose()
        {
            Directory.Delete(TempDir, true);
        }
    }
}
