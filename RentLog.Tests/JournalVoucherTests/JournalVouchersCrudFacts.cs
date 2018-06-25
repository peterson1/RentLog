using FluentAssertions;
using RentLog.ChequeVouchers;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.JournalVoucherTests
{
    [Trait("JV Crud Walkthrough", "Temp Copy")]
    public class JournalVouchersCrudFacts : TempCopyTestBase
    {
        protected override string SampleDirName => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "CRUD Walkthrough")]
        public async Task TestMethod00001()
        {
            var arg = GetTempSampleArgs();
            arg.Credentials.Roles = "Supervisor";
            var main = new MainWindowVM(arg, false);
            var rows = main.Journals.JournalRows;
            var crud = rows.Crud;
            main.SelectedIndex = 2;

            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(0);

            await crud.SetupForInsert();
            crud.Draft.SerialNum.Should().Be(1);
            crud.CanSave().Should().BeFalse();
            crud.Draft.Description = "some desc";
            crud.Draft.Amount = 1234;
            crud.CanSave().Should().BeTrue(crud.WhyInvalid);

            await crud.SaveDraftCmd.RunAsync();
            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(1);

            await crud.SetupForInsert();
            crud.Draft.SerialNum.Should().Be(2);
            crud.CanSave().Should().BeFalse();
            crud.Draft.Description = "another desc";
            crud.Draft.Amount = 5678;
            crud.CanSave().Should().BeTrue(crud.WhyInvalid);

            await crud.SaveDraftCmd.RunAsync();
            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(2);
        }
    }
}
