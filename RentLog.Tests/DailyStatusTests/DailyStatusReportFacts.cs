using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.DailyStatusTests
{
    [Trait("Daily Status", "Sample DBs")]
    public class DailyStatusReportFacts
    {
        const int DRY = 2;
        const int FRZ = 3;
        const int WET = 1;
        const int OFC = 0;


        [Fact(DisplayName = "Jul 3")]
        public void Jul3()
        {
            var arg = SampleDir.Jul3_GRY();
            var sut = DailyStatusReport.New(arg, 3.July(2018));

            sut.StallsInventory.Should().HaveCount(3);

            sut.SectionColxns.Should().HaveCount(4);

            sut.OtherColxns.Should().HaveCount(1);
            sut.OtherColxns.SummaryRows.Should().HaveCount(1);
            sut.OtherColxns.SummaryRows[0].Amount.Should().Be(434);

            sut.BankDeposits.Should().HaveCount(2);
            sut.BankDeposits.SummaryRows.Should().HaveCount(1);
            sut.BankDeposits.SummaryRows[0].Amount.Should().Be(14135);

            sut.CollectorsPerf.Should().HaveCount(1);
            //sut.CollectorsPerf[0].
        }
    }
}
