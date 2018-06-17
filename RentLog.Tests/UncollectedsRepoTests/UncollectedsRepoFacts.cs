using FluentAssertions.Extensions;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.UncollectedsRepoTests
{
    public class UncollectedsRepoFacts
    {
        private const int WET = 1;
        private const int DRY = 2;
        private const int FRZ = 3;

        [Fact(DisplayName = "Get Due")]
        public void TestMethod00001()
        {
            var arg = SampleDir.Lease197();
            var sut = arg.Collections.For(13.May(2018)).Uncollecteds[WET];

            //sut.GetDue()
        }
    }
}
