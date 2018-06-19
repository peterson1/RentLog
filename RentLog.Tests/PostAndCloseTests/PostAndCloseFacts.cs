using FluentAssertions;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using Xunit;

namespace RentLog.Tests.PostAndCloseTests
{
    [Trait("Post&Close", "Temp Copy")]
    public class PostAndCloseFacts : TempCopyTestBase
    {
        protected override string SampleDirName 
            => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "Post&Close June 17")]
        public void TestMethod00001()
        {
            var arg = GetSampleArgs();
            arg.Credentials.Roles = "Supervisor";

            //var sut
        }
    }
}
