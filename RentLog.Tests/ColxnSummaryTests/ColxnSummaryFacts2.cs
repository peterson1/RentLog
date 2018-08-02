using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.ColxnSummaryTests
{
    [Trait("Colxns Summary", "Sample DBs")]
    public class ColxnSummaryFacts2
    {
        [Fact(DisplayName = "Columns swapped (but not replicated)")]
        public void TestMethod00001()
        {
            var dir = SampleDir.Jul21_NoUncol();
            var sut = new ColxnSummaryReport(29.June(2018), 30.June(2018), dir);
            var row = sut[0];
            row.Date.Should().Be(29.June(2018));
            var sec = row[0];
            sec.Section.Name.Should().Be("DRY");
            sec.Rent.Should().Be(10328);
            //sut.ToExcel();
        }
    }
}
