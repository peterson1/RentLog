using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib45.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.ColxnSummaryTests
{
    [Trait("Colxns Summary", "Sample DBs")]
    public class ColxnSummaryFacts
    {
        const int DRY = 2;
        const int FRZ = 3;
        const int WET = 1;
        const int OFC = 0;

        const int CR  = 56;
        const int PRK = 55;
        const int PF  = 57;
        const int OTH = 60;


        [Fact(DisplayName = "May 4 to 6")]
        public void May4to7()
        {
            var arg = SampleArgs.Lease197();
            var sut = new ColxnSummaryReport(4.May(2018), 6.May(2018), arg);
            sut.Count.Should().Be(3);
            sut.DateRangeText.Should().Be("May 4, 2018 to May 6, 2018");

            sut.SectionTotals.Should().HaveCount(4);
            sut.SectionTotals.ContainsKey(DRY).Should().BeTrue();
            sut.SectionTotals.ContainsKey(FRZ).Should().BeTrue();
            sut.SectionTotals.ContainsKey(WET).Should().BeTrue();
            sut.SectionTotals.ContainsKey(OFC).Should().BeTrue();

            sut.SectionTotals[DRY].Rent.Should().Be(12721 + 9992);
            sut.SectionTotals[FRZ].Rent.Should().Be(0 + 0);
            sut.SectionTotals[WET].Rent.Should().Be(18199 + 5889);
            sut.SectionTotals[OFC].Rent.Should().Be(0 + 0);

            sut.SectionTotals[DRY].Rights.Should().Be(750 + 0);
            sut.SectionTotals[FRZ].Rights.Should().Be(0 + 0);
            sut.SectionTotals[WET].Rights.Should().Be(2508 + 5929);
            sut.SectionTotals[OFC].Rights.Should().Be(3000 + 0);

            sut.GLNames[CR].Should().Be("CR");
            sut.GLNames[PRK].Should().Be("Parking");
            sut.OtherTotals.Count.Should().Be(2);
            sut.OtherTotals[CR].Should().Be(465 + 666);
            sut.OtherTotals[PRK].Should().Be(549);

            sut.TotalCollections.Should().Be(68666);
            sut.TotalDeposits.Should().Be(68666);
        }


        [Fact(DisplayName = "Colxn Summary To Excel", Skip = "Undone")]
        public void ColxnSummaryToExcel()
        {
            var arg = SampleArgs.Lease197();
            var rep = new ColxnSummaryReport(3.May(2018), 12.May(2018), arg);
            rep.ToExcel();
        }
    }
}
