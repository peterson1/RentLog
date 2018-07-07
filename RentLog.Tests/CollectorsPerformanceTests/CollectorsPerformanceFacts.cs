using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.Reporters;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.CollectorsPerformanceTests
{
    [Trait("Collectors Performance", "Sample DBs")]
    public class CollectorsPerformanceFacts
    {
        [Fact(DisplayName = "Jul 3", Skip = "Undone")]
        public void Jul3()
        {
            var arg = SampleDir.Jul3_GRY();
            var db  = arg.Collections.For(3.July(2018));
            var sut = new CollectorsPerformanceReport(db);

            sut.Should().HaveCount(1);
        }
    }
}
