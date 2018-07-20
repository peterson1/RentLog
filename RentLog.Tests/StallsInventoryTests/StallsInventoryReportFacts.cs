using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using System.Linq;
using Xunit;

namespace RentLog.Tests.StallsInventoryTests
{
    [Trait("Stalls Inventory", "Sample DBs")]
    public class StallsInventoryReportFacts
    {
        const int DRY = 2;
        const int FRZ = 3;
        const int WET = 1;


        [Fact(DisplayName = "May 4")]
        public void May4()
        {
            var arg = SampleDir.Lease197();
            var db  = arg.Collections.For(4.May(2018));
            var mkt = arg.MarketState;
            var sut = new StallsInventoryReport(db, mkt);

            sut.Count.Should().Be(3);
            //sut.ContainsKey(DRY).Should().BeTrue();
            //sut.ContainsKey(FRZ).Should().BeTrue();
            //sut.ContainsKey(WET).Should().BeTrue();
            var dry = sut.Single(_ => _.Section.Id == DRY);
            var frz = sut.Single(_ => _.Section.Id == FRZ);
            var wet = sut.Single(_ => _.Section.Id == WET);

            dry.TotalCount.Should().Be(68);
            dry.TotalRent.Should().Be(11767);
            dry.Occupieds.Count.Should().Be(50);
            dry.OccupiedRent.Should().Be(8651);
            dry.OccupiedRate.Should().BeApproximately(0.74M, 0.01M);
            dry.Vacants.Count.Should().Be(18);
            dry.VacantRent.Should().Be(3116);
            dry.VacantRate.Should().BeApproximately(0.26M, 0.01M);

            frz.TotalCount.Should().Be(8);
            frz.TotalRent.Should().Be(0);
            frz.Occupieds.Count.Should().Be(0);
            frz.OccupiedRent.Should().Be(0);
            frz.OccupiedRate.Should().BeApproximately(0, 0.01M);
            frz.Vacants.Count.Should().Be(8);
            frz.VacantRent.Should().Be(0);
            frz.VacantRate.Should().BeApproximately(1, 0.01M);

            wet.TotalCount.Should().Be(102);
            wet.TotalRent.Should().Be(8520);
            wet.Occupieds.Count.Should().Be(83);
            wet.OccupiedRent.Should().Be(6960);
            wet.OccupiedRate.Should().BeApproximately(0.81M, 0.01M);
            wet.Vacants.Count.Should().Be(19);
            wet.VacantRent.Should().Be(1560);
            wet.VacantRate.Should().BeApproximately(0.19M, 0.01M);
        }


        [Fact(DisplayName = "Jul 3")]
        public void Jul3()
        {
            var arg = SampleDir.Jul3_GRY();
            var db  = arg.Collections.For(3.July(2018));
            var mkt = arg.MarketState;
            var sut = new StallsInventoryReport(db, mkt);

            sut.Count.Should().Be(3);
            //sut.ContainsKey(DRY).Should().BeTrue();
            //sut.ContainsKey(FRZ).Should().BeTrue();
            //sut.ContainsKey(WET).Should().BeTrue();
            var dry = sut.Single(_ => _.Section.Id == DRY);
            var frz = sut.Single(_ => _.Section.Id == FRZ);
            var wet = sut.Single(_ => _.Section.Id == WET);

            dry.TotalCount.Should().Be(68);
            dry.TotalRent.Should().Be(11767);
            dry.Occupieds.Count.Should().Be(38);
            dry.OccupiedRent.Should().Be(6615);
            dry.OccupiedRate.Should().BeApproximately(0.56M, 0.01M);
            dry.Vacants.Count.Should().Be(30);
            dry.VacantRent.Should().Be(5152);
            dry.VacantRate.Should().BeApproximately(0.44M, 0.01M);

            frz.TotalCount.Should().Be(8);
            frz.TotalRent.Should().Be(0);
            frz.Occupieds.Count.Should().Be(0);
            frz.OccupiedRent.Should().Be(0);
            frz.OccupiedRate.Should().BeApproximately(0, 0.01M);
            frz.Vacants.Count.Should().Be(8);
            frz.VacantRent.Should().Be(0);
            frz.VacantRate.Should().BeApproximately(1, 0.01M);

            wet.TotalCount.Should().Be(104);
            wet.TotalRent.Should().Be(8720);
            wet.Occupieds.Count.Should().Be(71);
            wet.OccupiedRent.Should().Be(6030);
            wet.OccupiedRate.Should().BeApproximately(0.68M, 0.01M);
            wet.Vacants.Count.Should().Be(33);
            wet.VacantRent.Should().Be(2690);
            wet.VacantRate.Should().BeApproximately(0.32M, 0.01M);
        }
    }
}
