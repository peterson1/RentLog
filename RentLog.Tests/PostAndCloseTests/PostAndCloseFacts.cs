using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Linq;
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
            arg.Collections.LastPostedDate().Should().Be(16.June(2018));
            arg.Collections.UnclosedDate().Should().Be(17.June(2018));
            arg.Balances.TotalOverdues().Rent.Should().Be(22_651.67M);
            arg.Balances.TotalOverdues().Rights.Should().Be(12_000M);

            arg.Credentials.Roles = "Supervisor";
            var vm = new MainWindowVM(17.June(2018), arg, false);

            await vm.RefreshCmd.RunAsync();

            vm.CanEncode.Should().BeFalse();
            vm.CanReview.Should().BeTrue();
            vm.PostAndClose.IsBalanced.Should().BeTrue();

            await vm.PostAndClose.RunPostAndClose();

            vm = null;
            arg = null;
            arg = GetTempSampleArgs();
            arg.Collections.LastPostedDate().Should().Be(17.June(2018));
            arg.Collections.UnclosedDate().Should().Be(18.June(2018));

            arg.Credentials.Roles = "Cashier";
            vm = new MainWindowVM(18.June(2018), arg, false);
            vm.ShouldClose.Should().BeFalse();
            vm.ColxnsDB.IsOpened().Should().BeFalse();

            await vm.RefreshCmd.RunAsync();

            vm.ColxnsDB.IsOpened().Should().BeTrue();
            arg.Balances.TotalOverdues().Rent.Should().BeApproximately(23_466.93M, 0.02M);
            arg.Balances.TotalOverdues().Rights.Should().Be(12_000M);

            var rows = arg.Passbooks.GetRepo(1).RowsFor(18.June(2018));
            rows.Should().HaveCount(1);
            rows.Sum(_ => _.Amount).Should().Be(19_165);
            rows.Last().RunningBalance.Should().Be(-13_651_235.56M);

            rows = arg.Passbooks.GetRepo(2).RowsFor(18.June(2018));
            rows.Should().HaveCount(1);
            rows.Sum(_ => _.Amount).Should().Be(582);
            rows.Last().RunningBalance.Should().Be(15_754);

            vm  = null;
            arg = null;
        }
    }
}
