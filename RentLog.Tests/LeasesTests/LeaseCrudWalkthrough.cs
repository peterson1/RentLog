using FluentAssertions;
using RentLog.LeasesCrud;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.LeasesTests
{
    [Trait("Lease Crud Walkthrough", "Temp Copy")]
    public class LeaseCrudWalkthrough : TempCopyTestBase
    {
        protected override string SampleDirName => SampleDir.JUN17_BALANCED;


        [Fact(DisplayName = "CRUD Walkthrough")]
        public async Task TestMethod00001()
        {
            var arg  = GetTempSampleArgs("Supervisor");
            var main = new MainWindowVM(arg, false);
            var rows = main.ActiveLeases.Rows;

            await main.RefreshCmd.RunAsync();
            rows.Should().HaveCount(113);
        }
    }
}
