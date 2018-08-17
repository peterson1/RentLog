using CommonTools.Lib11.DateTimeTools;
using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using System.Linq;
using Xunit;

namespace RentLog.Tests.GLRecapTests
{
    [Trait("GL Recap", "Sample DBs")]
    public class GLRecapFacts1
    {
        [Fact(DisplayName = "Aug 1, GRY")]
        public void TestMethod00001()
        {
            var dir = SampleDir.Aug16_GRY();
            var sut = new GLRecapReport(Month.July, 2018, dir);
            sut.Should().HaveCount(5);

            var equities = sut.ElementAt(0);
            equities.AccountType.Should().Be(GLAcctType.Equity);
            equities.Should().HaveCount(1);
            equities[0].Account.Name.Should().Be("Drawings");
            equities[0].TotalDebits.Should().Be(1223);
            equities[0].TotalCredits.Should().Be(1223);
            equities.SummaryRows[0].TotalDebits.Should().Be(1234);
            equities.SummaryRows[0].TotalCredits.Should().Be(1234);

            var assets = sut.ElementAt(1);


            sut.TotalDebits .Should().Be(1412);
            sut.TotalCredits.Should().Be(1412);
        }
    }
}
