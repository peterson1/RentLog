using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.CollectorsPerformanceTests
{
    [Trait("ColPerf Main Row", "Sample DBs")]
    public class ColPerfMainRowFacts
    {
        [Fact(DisplayName = "Jul 3: Main Row ")]
        public void Jul3()
        {
            var arg = SampleDir.Jul3_GRY();
            var db  = arg.Collections.For(3.July(2018));
            var sut = new CollectorsPerformanceReport(db, arg.MarketState);

            sut.Should().HaveCount(1);
            sut[0].Collector.Should().NotBeNull();
            sut[0].Collector.Name.Should().Contain("Pasalu");
            sut[0].Assignment.Should().Be("DRY, WET");

            var covr = sut[0].StallCoverage;
            covr.ActiveStallsCount.Should().Be(101);
            covr.CollectedsCount  .Should().Be(94);
            covr.UncollectedsCount.Should().Be(7);
            covr.NoOperationsCount.Should().Be(8);
            covr.CoverageRate     .Should().BeApproximately(0.93M, 0.01M);

            var rent = sut[0].RentBill;
            rent.Target        .Should().Be(15013.1M);
            rent.NoExcess      .Should().Be(10574.35M);
            rent.UnderpaidCount.Should().Be(15);
            rent.UnderpaidTotal.Should().Be(4438.75M);
            rent.OverpaidCount .Should().Be(55);
            rent.OverpaidTotal .Should().Be(1240.65M);
            rent.PerfRate      .Should().BeApproximately(0.70M, 0.01M);

            var rights = sut[0].RightsBill;
            rights.Target        .Should().Be(1881);
            rights.NoExcess      .Should().Be(116);
            rights.UnderpaidCount.Should().Be(31);
            rights.UnderpaidTotal.Should().Be(1840);
            rights.OverpaidCount .Should().Be(0);
            rights.OverpaidTotal .Should().Be(0);
            rights.PerfRate      .Should().BeApproximately(0.06M, 0.01M);
        }
    }
}
