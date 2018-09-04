using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using System.Linq;
using Xunit;

namespace RentLog.Tests.CollectorsPerformanceTests
{
    [Trait("ColPerf Sub Row", "Sample DBs")]
    public class ColPerfSubRowFacts
    {
        [Fact(DisplayName = "Jul 3: Rent")]
        public void Jul3()
        {
            var arg = SampleDir.Jul3_GRY();
            var db  = arg.Collections.For(3.July(2018));
            var sut = new CollectorsPerformanceReport(db, arg.MarketState);

            sut.Should().HaveCount(1);
            sut[0].Should().HaveCount(94);

            // sample exact rent payment
            var row = sut[0].Single(_ => _.Lease.Id == 104);
            row.Stall.Name   .Should().Be("WET 089");
            row.Rent.Target  .Should().Be(90);
            row.Rent.Actual  .Should().Be(90);
            row.Rent.NoExcess.Should().Be(90);
            row.Rent.Shortage.Should().Be(0);
            row.Rent.Overage .Should().Be(0);

            // sample rent overpaid 1
            row = sut[0].Single(_ => _.Lease.Id == 80);
            row.Stall.Name   .Should().Be("DRY 040");
            row.Rent.Target  .Should().Be(63);
            row.Rent.Actual  .Should().Be(203);
            row.Rent.NoExcess.Should().Be(63);
            row.Rent.Shortage.Should().Be(0);
            row.Rent.Overage .Should().Be(140);

            // sample rent underpaid 1
            row = sut[0].Single(_ => _.Lease.Id == 48);
            row.Stall.Name   .Should().Be("WET 102");
            row.Rent.Target  .Should().Be(208.09M);
            row.Rent.Actual  .Should().Be(205);
            row.Rent.NoExcess.Should().Be(205);
            row.Rent.Shortage.Should().Be(3.09M);
            row.Rent.Overage .Should().Be(0);

            // sample w/ negative rent balance
            row = sut[0].Single(_ => _.Lease.Id == 81);
            row.Stall.Name   .Should().Be("WET 024");
            row.Rent.Target  .Should().Be(0);
            row.Rent.Actual  .Should().Be(90);
            row.Rent.NoExcess.Should().Be(0);
            row.Rent.Shortage.Should().Be(0);
            row.Rent.Overage .Should().Be(90);
        }


        [Fact(DisplayName = "Aug. 27 Rights")]
        public void Aug27Rights()
        {
            var dir = SampleDir.Aug27_GRY();
            var db  = dir.Collections.For(27.August(2018));
            var sut = new CollectorsPerformanceReport(db, dir.MarketState);
            sut.Should().HaveCount(1);
            sut[0].Should().HaveCount(75);

            var row = sut[0].Single(_ => _.Lease.Id == 169); //DRY 056
            row.Rights.Target.Should().Be(234);
        }
    }
}
