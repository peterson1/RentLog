using CommonTools.Lib45.FileSystemTools;
using FluentAssertions;
using System.IO;

namespace CommonTools.Tests.LiteDbToolsTests
{
    class TemmpCopyOf
    {
        public static string DirPath = @"..\..\LiteDbToolsTests";


        public static string MakeTempCopy(string fileName)
        {
            var orig = Path.Combine(DirPath, fileName);
            File.Exists(orig).Should().BeTrue();

            var temp = orig.MakeTempCopy();
            File.Exists(temp).Should().BeTrue();

            return temp;
        }


        public static string FMEY_20180924()
            => TemmpCopyOf.MakeTempCopy("F_MEY_2018-09-24_Solo.ldb");
    }
}
