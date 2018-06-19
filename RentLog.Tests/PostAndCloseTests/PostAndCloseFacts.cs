using FluentAssertions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.PostAndCloseTests
{
    [Trait("Post&Close", "Temp Copy")]
    public class PostAndCloseFacts : TempCopyTestBase
    {
        protected override string SampleDirName 
            => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "Post&Close June 17")]
        public async Task TestMethod00001()
        {
            var arg = GetTempSampleArgs();
            arg.Credentials.Roles = "Supervisor";
            var mkt = arg.MarketState.DatabasePath;

            var dte = arg.Collections.UnclosedDate();
            var vm = new MainWindowVM(dte, arg, false);

            await vm.RefreshCmd.RunAsync();

            vm.CanEncode.Should().BeFalse();
            vm.CanReview.Should().BeTrue();
            vm.PostAndClose.IsBalanced.Should().BeTrue();
            //vm.PostAndClose.PostAndCloseCmd.IsBusy
        }
    }
}
