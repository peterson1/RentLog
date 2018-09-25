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
    public class PostAndCloseFacts1 : TempCopyTestBase
    {
        protected override string SampleDirName 
            => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "Post&Close June 17")]
        public async Task TestMethod00001()
        {
            var cashierArg = GetTempSampleArgs("Cashier");
            cashierArg.Collections.LastPostedDate().Should().Be(16.June(2018));
            cashierArg.Collections.UnclosedDate().Should().Be(17.June(2018));
            cashierArg.Balances.TotalOverdues().Rent.Should().Be(22_651.67M);
            cashierArg.Balances.TotalOverdues().Rights.Should().Be(34_000M);

            var cashierVm = new MainWindowVM(17.June(2018), cashierArg, false);
            await cashierVm.RefreshCmd.RunAsync();
            cashierVm.ApprovalAwaiter.SubmitForApproval();

            var suprvsrArg = GetTempSampleArgs("Supervisor");
            var suprvsrVm = new MainWindowVM(17.June(2018), suprvsrArg, false);
            await suprvsrVm.RefreshCmd.RunAsync();
            suprvsrVm.CanEncode.Should().BeFalse();
            suprvsrVm.CanReview.Should().BeTrue();
            suprvsrVm.PostAndClose.CanPostAndClose().Should().BeTrue();
            suprvsrVm.PostAndClose.RunPostAndClose();

            await Task.Delay(1000 * 5);
            cashierVm  = null;
            cashierArg = null;
            cashierArg = GetTempSampleArgs("Cashier");
            cashierArg.Collections.LastPostedDate().Should().Be(17.June(2018));
            cashierArg.Collections.UnclosedDate().Should().Be(18.June(2018));

            cashierVm = new MainWindowVM(18.June(2018), cashierArg, false);
            cashierVm.ShouldClose.Should().BeFalse();
            cashierVm.ColxnsDB.IsOpened().Should().BeFalse();
            await cashierVm.RefreshCmd.RunAsync();
            await Task.Delay(1000 * 5);

            cashierVm.ColxnsDB.IsOpened().Should().BeTrue();
            cashierArg.Balances.TotalOverdues().Rent.Should().BeApproximately(23_466.93M, 0.02M);
            //cashierArg.Balances.TotalOverdues().Rights.Should().Be(12_000M);

            //var rows = cashierArg.Passbooks.GetRepo(1).RowsFor(18.June(2018));
            //rows.Should().HaveCount(1);
            //rows.Sum(_ => _.Amount).Should().Be(19_165);
            //rows.Last().RunningBalance.Should().Be(-13_651_235.56M);

            //rows = cashierArg.Passbooks.GetRepo(2).RowsFor(18.June(2018));
            //rows.Should().HaveCount(1);
            //rows.Sum(_ => _.Amount).Should().Be(582);
            //rows.Last().RunningBalance.Should().Be(15_754);
        }
    }
}
