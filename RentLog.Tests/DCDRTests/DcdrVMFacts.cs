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


        [Fact(DisplayName = "Edit Cleared Date - same month")]
        public async Task TestMethod00001()
        {
            var arg  = GetTempSampleArgs("Admin");
            var main = new MainWindowVM(arg, false);
            var rows = main.DcdrReport.PassbookRows;
            main.DateRange.Start = 1.March(2017);
            main.SelectedTab   =  MainTabs.DcdrReport; // DC-DR tab
            await main.RefreshCmd.RunAsync();
            rows.ItemsList.Should().HaveCount(946);

            var row = rows.ItemsList[0];
            row.Subject.Should().Be("Cheque for Restituto Linquico");
            row.TransactionDate.Should().Be(12.March(2017));

            var crud = rows.LaunchChequeVoucherViewer(row, false);
            crud.CanEditClearedDate().Should().BeTrue();
            crud.PickedDate = 30.March(2017);
            await crud.EditClearedDateCmd.RunAsync();

            await main.RefreshCmd.RunAsync();

            row = rows.ItemsList[0];
            row.Subject.Should().Be("Cheque for Restituto Linquico");
            row.TransactionDate.Should().Be(30.March(2017));
            rows.ItemsList.Should().HaveCount(946);
        }


        [Fact(DisplayName = "Edit Cleared Date - diff month")]
        public async Task TestMethod00002()
        {
            var arg  = GetTempSampleArgs("Admin");
            var main = new MainWindowVM(arg, false);
            var rows = main.DcdrReport.PassbookRows;
            main.DateRange.Start = 1.March(2017);
            main.SelectedTab =  MainTabs.DcdrReport; // DC-DR tab
            await main.RefreshCmd.RunAsync();
            rows.ItemsList.Should().HaveCount(946);

            var row = rows.ItemsList[0];
            row.Subject.Should().Be("Cheque for Restituto Linquico");
            row.TransactionDate.Should().Be(12.March(2017));

            var crud = rows.LaunchChequeVoucherViewer(row, false);
            crud.CanEditClearedDate().Should().BeTrue();
            crud.PickedDate = 3.April(2017);
            await crud.EditClearedDateCmd.RunAsync();

            await main.RefreshCmd.RunAsync();

            row = rows.ItemsList[0];
            row.Subject.Should().Be("Cheque for Restituto Linquico");
            row.TransactionDate.Should().Be(3.April(2017));
            rows.ItemsList.Should().HaveCount(946);
        }


        [Fact(DisplayName = "Edit Passbook row - Description")]
        public async Task TestMethod00003()
        {
            var arg  = GetTempSampleArgs("Cashier");
            var main = new MainWindowVM(arg, false);
            var vm   = main.DcdrReport.PassbookRows;
            main.DateRange.Start = 1.March(2017);
            main.SelectedTab =  MainTabs.DcdrReport; // DC-DR tab
            await main.RefreshCmd.RunAsync();
            vm.ItemsList.Should().HaveCount(946);

            var row = vm.ItemsList[1];
            row.Description.Should().Be("Check Booklet-");
            row.TransactionDate.Should().Be(27.April(2017));

            var crud = vm.CreateCrud();
            crud.SetupForUpdate(row);
            crud.Draft.Description = "a different desc";
            await crud.SaveDraftCmd.RunAsync();
            await main.RefreshCmd.RunAsync();

            row = vm.ItemsList[1];
            row.Description.Should().Be("a different desc");
            row.TransactionDate.Should().Be(27.April(2017));
            vm.ItemsList.Should().HaveCount(946);
        }


        [Fact(DisplayName = "Edit Passbook row - Date - next month")]
        public async Task TestMethod00004()
        {
            var arg = GetTempSampleArgs("Cashier");
            var main = new MainWindowVM(arg, false);
            var vm = main.DcdrReport.PassbookRows;
            main.DateRange.Start = 1.March(2017);
            main.SelectedTab =  MainTabs.DcdrReport; // DC-DR tab
            await main.RefreshCmd.RunAsync();
            vm.ItemsList.Should().HaveCount(946);

            var row = vm.ItemsList[1];
            row.Description.Should().Be("Check Booklet-");
            row.TransactionDate.Should().Be(27.April(2017));

            var crud = vm.CreateCrud();
            crud.SetupForUpdate(row);
            crud.TransactionDate = 3.May(2017);
            await crud.SaveDraftCmd.RunAsync();
            await main.RefreshCmd.RunAsync();

            vm.ItemsList.Should().HaveCount(946);
            row = vm.ItemsList[1];
            row.Description.Should().Be("Interest");
            row.TransactionDate.Should().Be(30.April(2017));

            row = vm.ItemsList[4];
            row.Description.Should().Be("Check Booklet-");
            row.TransactionDate.Should().Be(3.May(2017));
        }
    }
}
