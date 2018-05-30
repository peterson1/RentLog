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


        [Fact(DisplayName = "May 4")]
        public void May4()
        {
            var arg = SampleArgs.HelenAblen_Dry8();
            var sut = DailyColxnsReport.Load(arg, 4.May(2018));

            sut.Should().HaveCount(4);
            sut.ContainsKey(DRY).Should().BeTrue();
            sut.ContainsKey(FRZ).Should().BeTrue();
            sut.ContainsKey(WET).Should().BeTrue();
            sut.ContainsKey(OFC).Should().BeTrue();

            sut[DRY].Rent.Should().Be(5821);
            sut[FRZ].Rent.Should().Be(0);
            sut[WET].Rent.Should().Be(10164);
            sut[OFC].Rent.Should().Be(0);
        }
    }
}
