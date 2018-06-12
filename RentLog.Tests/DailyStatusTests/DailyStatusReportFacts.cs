using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.DailyStatusTests
{
    //[Trait("Daily Status", "Sample DBs")]
    public class DailyStatusReportFacts
    {
        //[Fact(DisplayName = "May 4", Skip = "Undone")]
        public void May4()
        {
            var arg = SampleArgs.Lease197();
            var sut = new DailyStatusReport(4.May(2018), arg);

        }
    }
}
