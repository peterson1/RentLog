using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.ColxnSummaryTests
{
    [Trait("Colxns Summary", "Sample DBs")]
    public class ColxnSummaryFacts
    {
        [Fact(DisplayName = "May 4 to 7", Skip = "Undone")]
        public void May4to7()
        {
            var arg = SampleArgs.HelenAblen_Dry8();
            var sut = new ColxnSummaryReport(4.May(2018), 7.May(2018), arg);

        }
    }
}
