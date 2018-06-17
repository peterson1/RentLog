using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.StallsRepoTests
{
    [Trait("Stalls Repo", "Sample Dir")]
    public class StallsRepoReaderFacts
    {
        [Fact(DisplayName = "Find by ID includes Section object")]
        public void FindbyIDIncludesSectionobject()
        {
            var arg = SampleDir.Lease197();
            var sut = arg.MarketState.Stalls;
            var rec = sut.Find(1, true);
            rec.Section.Should().NotBeNull();
            rec.Section.Name.Should().Be("WET");
        }


        [Fact(DisplayName = "GetAll includes Section object")]
        public void GetAllIncludesSectionobject()
        {
            var arg = SampleDir.Lease197();
            var sut = arg.MarketState.Stalls;
            var all = sut.GetAll();
            foreach (var stall in all)
            {
                stall.Section.Should().NotBeNull();
                stall.Section.Name.Should().NotBeEmpty();
            }
        }
    }
}
