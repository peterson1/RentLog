using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using System.Linq;
using Xunit;

namespace RentLog.Tests.CollectorsPerformanceTests
{
    [Trait("Collectors Performance", "Sample DBs")]
    public class CollectorsPerformanceFacts
    {
        [Fact(DisplayName = "Jul 3", Skip = "Undone")]
        public void Jul3()
        {
            var arg = SampleDir.Jul3_GRY();
            var db  = arg.Collections.For(3.July(2018));
            var sut = new CollectorsPerformanceReport(db);

            sut.Should().HaveCount(1);
            sut[0].Collector.Should().NotBeNull();
            sut[0].Collector.Name.Should().Contain("Pasalu");
            sut[0].Assignment.Should().Be("DRY, WET");
            sut[0].Should().HaveCount(94);

            // sample exact rent payment
            var row = sut[0].Single(_ => _.Lease.Id == 104);
            row.Stall.Name.Should().Be("WET 089");
            row.Rent.Target.Should().Be(90);
            row.Rent.Actual.Should().Be(90);
            row.Rent.NoExcess.Should().Be(90);
            row.Rent.Shortage.Should().Be(0);
            row.Rent.Overage.Should().Be(0);

            // sample overpaid 1
            row = sut[0].Single(_ => _.Lease.Id == 80);
            row.Stall.Name.Should().Be("DRY 040");
            row.Rent.Target.Should().Be(63);
        }
    }
}
