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

            var expctd = "\"C:\\a\\b c\\d.exe\""
                      + " -arg1=\"C:\\a\\b c\\db.ldb\""
                      + " -arg2=\"abcdefg\"";

            args.QuotifyCommandLineArgs().Should().Be(expctd);
        }


        [Fact(DisplayName = "exe path + 1 arg")]
        public void TestMethod00002()
        {
            var args = new string[] { @"C:\a\b c\d.exe",
                                      @"-arg1=C:\a\b c\db.ldb" };

            var expctd = "\"C:\\a\\b c\\d.exe\""
                      + " -arg1=\"C:\\a\\b c\\db.ldb\"";

            args.QuotifyCommandLineArgs().Should().Be(expctd);
        }


        [Fact(DisplayName = "exe path only")]
        public void TestMethod00003()
        {
            var args   = new string[] { @"C:\a\b c\d.exe" };
            var expctd = "\"C:\\a\\b c\\d.exe\"";

            args.QuotifyCommandLineArgs().Should().Be(expctd);
        }
    }
}
