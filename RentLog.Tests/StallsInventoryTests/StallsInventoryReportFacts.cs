using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
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
            var sut = new StallsInventoryReport(4.May(2018), arg);

            sut.Count.Should().Be(3);
            sut.ContainsKey(DRY).Should().BeTrue();
            sut.ContainsKey(FRZ).Should().BeTrue();
            sut.ContainsKey(WET).Should().BeTrue();

            sut[DRY].TotalCount.Should().Be(68);
            sut[DRY].TotalRent.Should().Be(11767);
            sut[DRY].Occupieds.Count.Should().Be(50);
            sut[DRY].OccupiedRent.Should().Be(8651);
            sut[DRY].OccupiedRate.Should().BeApproximately(0.74M, 0.01M);
            sut[DRY].Vacants.Count.Should().Be(18);
            sut[DRY].VacantRent.Should().Be(3116);
            sut[DRY].VacantRate.Should().BeApproximately(0.26M, 0.01M);

            sut[FRZ].TotalCount.Should().Be(8);
            sut[FRZ].TotalRent.Should().Be(0);
            sut[FRZ].Occupieds.Count.Should().Be(0);
            sut[FRZ].OccupiedRent.Should().Be(0);
            sut[FRZ].OccupiedRate.Should().BeApproximately(0, 0.01M);
            sut[FRZ].Vacants.Count.Should().Be(8);
            sut[FRZ].VacantRent.Should().Be(0);
            sut[FRZ].VacantRate.Should().BeApproximately(1, 0.01M);

            sut[WET].TotalCount.Should().Be(102);
            sut[WET].TotalRent.Should().Be(8520);
            sut[WET].Occupieds.Count.Should().Be(83);
            sut[WET].OccupiedRent.Should().Be(6960);
            sut[WET].OccupiedRate.Should().BeApproximately(0.81M, 0.01M);
            sut[WET].Vacants.Count.Should().Be(19);
            sut[WET].VacantRent.Should().Be(1560);
            sut[WET].VacantRate.Should().BeApproximately(0.19M, 0.01M);
        }
    }
}
