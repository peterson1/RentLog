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

            var sub = sut.ElementAt(0);
            sub.AccountType.Should().Be(GLAcctType.Equity);
            sub.Should().HaveCount(3);
            sub.SummaryRows[0].TotalDebits.Should().Be(1234);
            sub.SummaryRows[0].TotalCredits.Should().Be(1234);


            sut.TotalDebits .Should().Be(1412);
            sut.TotalCredits.Should().Be(1412);
        }
    }
}
