using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using System.Linq;
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
            var arg = SampleDir.Lease197();
            var sut = new DailyColxnsReport(4.May(2018), arg);

            sut.Should().HaveCount(4);
            //sut.ContainsKey(DRY).Should().BeTrue();
            //sut.ContainsKey(FRZ).Should().BeTrue();
            //sut.ContainsKey(WET).Should().BeTrue();
            //sut.ContainsKey(OFC).Should().BeTrue();
            var dry = sut.Single(_ => _.Section.Id == DRY);
            var frz = sut.Single(_ => _.Section.Id == FRZ);
            var wet = sut.Single(_ => _.Section.Id == WET);
            var ofc = sut.Single(_ => _.Section.Id == OFC);

            dry.Rent.Should().Be(5821);
            frz.Rent.Should().Be(0);
            wet.Rent.Should().Be(10164);
            ofc.Rent.Should().Be(0);
            sut.TotalRent.Should().Be(15985);

            dry.Rights.Should().Be(750);
            frz.Rights.Should().Be(0);
            wet.Rights.Should().Be(1679);
            ofc.Rights.Should().Be(0);
            sut.TotalRights.Should().Be(2429);

            dry.Electric.Should().Be(831);
            frz.Electric.Should().Be(0);
            wet.Electric.Should().Be(965);
            ofc.Electric.Should().Be(0);
            sut.TotalElectric.Should().Be(1796);

            dry.Water.Should().Be(0);
            frz.Water.Should().Be(0);
            wet.Water.Should().Be(0);
            ofc.Water.Should().Be(0);
            sut.TotalWater.Should().Be(0);

            dry.Ambulant.Should().Be(0);
            frz.Ambulant.Should().Be(168);
            wet.Ambulant.Should().Be(355);
            ofc.Ambulant.Should().Be(0);
            sut.TotalAmbulant.Should().Be(523);

            dry.Total.Should().Be(    7_402);
            frz.Total.Should().Be(      168);
            wet.Total.Should().Be(   13_163);
            ofc.Total.Should().Be(        0);
            sut.SectionsTotal.Should().Be(20_733);

            sut.Others.Count.Should().Be(1);
            sut.Others[CR].Should().Be(465);

            sut.CollectionsSum.Should().Be(21_198);
            sut.DepositsSum.Should().Be(21_198);
        }
    }
}
