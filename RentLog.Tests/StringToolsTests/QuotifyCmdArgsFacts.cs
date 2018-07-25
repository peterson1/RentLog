using CommonTools.Lib11.StringTools;
using FluentAssertions;
using Xunit;

namespace RentLog.Tests.StringToolsTests
{
    [Trait("QuotifyCommandLineArgs", "StringTools")]
    public class QuotifyCmdArgsFacts
    {
        [Fact(DisplayName = "exe path + 2 args")]
        public void TestMethod00001()
        {
            var args = new string[] { @"C:\a\b c\d.exe",
                                    @"-arg1=C:\a\b c\db.ldb",
                                     "-arg2=abcdefg"};

            args.QuotifyCommandLineArgs().Should().Be(
                //"\"C:\\a\\b c\\d.exe\""
                "" + "-arg1=\"C:\\a\\b c\\db.ldb\""
                + " -arg2=\"abcdefg\"");
        }


        [Fact(DisplayName = "exe path + 1 arg")]
        public void TestMethod00002()
        {
            var args = new string[] { @"C:\a\b c\d.exe",
                                      @"-arg1=C:\a\b c\db.ldb" };

            args.QuotifyCommandLineArgs().Should().Be(
                    //"\"C:\\a\\b c\\d.exe\""
                      "" + "-arg1=\"C:\\a\\b c\\db.ldb\"");
        }


        [Fact(DisplayName = "exe path + 1 arg (not k=v pair)")]
        public void TestMethod00002b()
        {
            var args = new string[] { @"C:\a\b c\d.exe",
                                      "non-kvp-arg" };

            args.QuotifyCommandLineArgs().Should().Be(
                //"\"C:\\a\\b c\\d.exe\""
                "" + "\"non-kvp-arg\"");
        }


        [Fact(DisplayName = "exe path only")]
        public void TestMethod00003()
        {
            var args   = new string[] { @"C:\a\b c\d.exe" };
            args.QuotifyCommandLineArgs().Should().Be("");
                //"\"C:\\a\\b c\\d.exe\"");
        }


        [Fact(DisplayName = "kvp arg has EQ char")]
        public void TestMethod00004()
        {
            var args = new string[] { @"C:\a\b c\d.exe",
                                      @"-arg1=abc123=" };

            args.QuotifyCommandLineArgs().Should().Be(
                //"\"C:\\a\\b c\\d.exe\""
                      "" + "-arg1=\"abc123=\"");
        }


        [Fact(DisplayName = "kvp arg has 2 EQ chars")]
        public void TestMethod00005()
        {
            var args = new string[] { @"C:\a\b c\d.exe",
                                      @"-arg1=abc123==" };

            args.QuotifyCommandLineArgs().Should().Be(
                //"\"C:\\a\\b c\\d.exe\""
                     "" + "-arg1=\"abc123==\"");
        }
    }
}
