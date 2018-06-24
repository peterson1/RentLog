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
            crud.SetupForInsert();
            crud.Draft.SerialNum.Should().Be(1);
        }
    }
}
