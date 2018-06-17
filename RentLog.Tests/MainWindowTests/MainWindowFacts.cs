using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.Cashiering;
using RentLog.Tests.SampleDBs;
using System.Threading.Tasks;
using Xunit;

namespace RentLog.Tests.MainWindowTests
{
    [Trait("Main Windows", "Sample DBs")]
    public class MainWindowFacts
    {
        [Fact(DisplayName = "Cashiering Loads OK")]
        public async Task TestMethod00001()
        {
            var arg = SampleDir.Lease197();
            arg.Collections.ExecutionTimeOf(_ => _.UnclosedDate())
                .Should().BeLessThan(500.Milliseconds());

            var dte = arg.Collections.UnclosedDate();
            var sut = new MainWindowVM(dte, arg, false);
            await sut.RefreshCmd.RunAsync();
            sut.SectionTabs.Should().HaveCount(3);
        }
    }
}
