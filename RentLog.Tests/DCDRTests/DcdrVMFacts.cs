using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.ChequeVouchers;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.DCDRTests
{
    [Trait("DCDR VM", "Temp Copy")]
    public class DcdrVMFacts : TempCopyTestBase
    {
        protected override string SampleDirName => SampleDir.JUL09_F_GRY;


        [Fact(DisplayName = "Applies Base Balance", Skip = "unnecessary?")]
        public async Task TestMethod00001()
        {
            var arg = GetTempSampleArgs("Supervisor");
            //arg.Passbooks.GetRepo(1).GetBaseBalance().Should().Be(1_000_000);

            var vm  = new MainWindowVM(arg, false);
            vm.DateRange.Start = 1.March(2017);
            vm.SelectedIndex   = 1; // DC-DR tab
            await vm.RefreshCmd.RunAsync();
            vm.DcdrReport.PassbookRows.ItemsList.Should().HaveCount(946);

            var row = vm.DcdrReport.PassbookRows.ItemsList[0];
            row.Subject.Should().Be("Cheque for Restituto Linquico");

            //vm.DcdrReport.RecomputeBalances
        }


        [Fact(DisplayName = "Edit Cleared Date - same month", Skip = "Undone")]
        public async Task TestMethod00002()
        {
            var arg = GetTempSampleArgs("Cashier");
            var vm = new MainWindowVM(arg, false);
            await vm.RefreshCmd.RunAsync();

            //vm.DcdrReport.RecomputeBalances
        }
    }
}
