using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.DailyColxnsTests
{
    [Trait("Daily Colxns", "Sample DBs")]
    public class DailyColxnsFacts
    {
        const int DRY = 2;
        const int FRZ = 3;
        const int WET = 1;
        const int OFC = 0;

        const int CR  = 56;
        const int PRK = 55;
        const int PF  = 57;
        const int OTH = 60;

        [Fact(DisplayName = "May 4")]
        public void May4()
        {
            var arg = SampleArgs.Lease197();
            var sut = new DailyColxnsReport(4.May(2018), arg);

            sut.Should().HaveCount(4);
            sut.ContainsKey(DRY).Should().BeTrue();
            sut.ContainsKey(FRZ).Should().BeTrue();
            sut.ContainsKey(WET).Should().BeTrue();
            sut.ContainsKey(OFC).Should().BeTrue();

            sut[DRY].Rent.Should().Be(5821);
            sut[FRZ].Rent.Should().Be(0);
            sut[WET].Rent.Should().Be(10164);
            sut[OFC].Rent.Should().Be(0);
            sut.TotalRent.Should().Be(15985);

            sut[DRY].Rights.Should().Be(750);
            sut[FRZ].Rights.Should().Be(0);
            sut[WET].Rights.Should().Be(1679);
            sut[OFC].Rights.Should().Be(0);
            sut.TotalRights.Should().Be(2429);

            sut[DRY].Electric.Should().Be(831);
            sut[FRZ].Electric.Should().Be(0);
            sut[WET].Electric.Should().Be(965);
            sut[OFC].Electric.Should().Be(0);
            sut.TotalElectric.Should().Be(1796);

            sut[DRY].Water.Should().Be(0);
            sut[FRZ].Water.Should().Be(0);
            sut[WET].Water.Should().Be(0);
            sut[OFC].Water.Should().Be(0);
            sut.TotalWater.Should().Be(0);

            sut[DRY].Ambulant.Should().Be(0);
            sut[FRZ].Ambulant.Should().Be(168);
            sut[WET].Ambulant.Should().Be(355);
            sut[OFC].Ambulant.Should().Be(0);
            sut.TotalAmbulant.Should().Be(523);

            sut[DRY].Total.Should().Be(    7_402);
            sut[FRZ].Total.Should().Be(      168);
            sut[WET].Total.Should().Be(   13_163);
            sut[OFC].Total.Should().Be(        0);
            sut.SectionsTotal.Should().Be(20_733);

            sut.Others.Count.Should().Be(1);
            sut.Others[CR].Should().Be(465);

            sut.CollectionsSum.Should().Be(21_198);
            sut.DepositsSum.Should().Be(21_198);
        }
    }
}
