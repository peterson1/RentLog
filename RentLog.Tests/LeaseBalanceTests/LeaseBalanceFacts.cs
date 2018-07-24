using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using Xunit;

namespace RentLog.Tests.LeaseBalanceTests
{
    [Trait("Lease Balance Updater", "Sample Dir")]
    public class LeaseBalanceFacts : TempCopyTestBase
    {
        protected override string SampleDirName => SampleDir.JUL13_F_MEY;


        [Fact(DisplayName = "Shows July 12")]
        public void TestMethod00001()
        {
            var dir  = GetTempSampleArgs("Supervisor");
            var repo = dir.Balances.GetRepo(349); // DRY 030
            repo.GetAll().Count.Should().Be(14);
            repo.UpdateFrom(28.June(2018));
            repo.GetAll().Count.Should().Be(16);
        }
    }
}
