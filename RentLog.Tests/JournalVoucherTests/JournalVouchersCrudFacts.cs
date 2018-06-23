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


        [Fact(DisplayName = "CRUD Walkthrough", Skip ="undone")]
        public async Task TestMethod00001()
        {
            var arg = GetTempSampleArgs();
            arg.Credentials.Roles = "Supervisor";
            var vm = new MainWindowVM(arg, false);

            await vm.RefreshCmd.RunAsync();

            //vm.VoucherReqs
        }
    }
}
