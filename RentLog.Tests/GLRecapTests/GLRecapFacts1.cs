using CommonTools.Lib11.DateTimeTools;
using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib45.Reporters;
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
            var dir = SampleDir.Aug27_GRY();
            var sut = new GLRecapReport(Month.July, 2018, dir);
            sut.Should().HaveCount(5);

            var equities = sut.ElementAt(0);
            equities.AccountType.Should().Be(GLAcctType.Equity);
            equities.Should().HaveCount(1);
            equities[0].Account.Name.Should().Be("Drawings");
            equities[0].TotalDebits.Should().Be(230780);
            equities[0].TotalCredits.Should().Be(0);
            equities[0].Should().HaveCount(2);
            equities.TotalDebits.Should().Be(230780);
            equities.TotalCredits.Should().Be(0);

            var assets = sut.ElementAt(1);
            assets.AccountType.Should().Be(GLAcctType.Asset);
            assets.Should().HaveCount(5);
            assets[1].Account.Name.Should().Be("Advances to Officers and Employees");
            assets[1].TotalDebits.Should().Be(1135);
            assets[1].TotalCredits.Should().Be(0);
            assets[1].Should().HaveCount(1);
            assets.TotalDebits.Should().Be(111455.83M);
            assets.TotalCredits.Should().Be(756405.14M);

            sut.TotalDebits .Should().Be(770000.23M);
            sut.TotalCredits.Should().Be(770000.23M);
        }


        [Fact(DisplayName = "GL Recap to Excel")]
        public void TestMethod00002()
        {
            var dir = SampleDir.Aug27_GRY();
            var sut = new GLRecapReport(Month.July, 2018, dir);
            sut.ToExcel();
        }
    }
}
