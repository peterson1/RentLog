using FluentAssertions;
using CommonTools.Lib11.DateTimeTools;
using FluentAssertions.Extensions;
using RentLog.Tests.SampleDBs;
using RentLog.Tests.TestTools;
using System.Linq;
using Xunit;
using RentLog.DomainLib11.Models;

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
            var rows = repo.GetAll();
            rows.Count.Should().Be(16);

            rows[1].Id.Should().Be(12.July(2018).DaysSinceMin());
            var rent = rows[1].For(BillCode.Rent);
            rent.Should().NotBeNull();
            rent.TotalPayments.Should().Be(100);

            var electric = rows[1].For(BillCode.Electric);
            electric.Should().NotBeNull();
            electric.TotalPayments.Should().Be(16);
        }
    }
}
