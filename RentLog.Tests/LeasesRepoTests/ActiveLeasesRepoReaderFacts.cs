using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.LeasesRepoTests
{
    [Trait("Active Leases Repo", "Sample Dir")]
    public class ActiveLeasesRepoReaderFacts
    {
        [Fact(DisplayName = "Find by ID includes Section object")]
        public void FindbyIDIncludesSectionobject()
        {
            var arg = SampleDir.Lease197(out LeaseDTO lse);
            var sut = arg.MarketState.ActiveLeases;
            var rec = sut.Find(10, true);
            rec.Stall.Section.Should().NotBeNull();
            rec.Stall.Section.Name.Should().Be("WET");
        }


        [Fact(DisplayName = "GetAll includes Section object")]
        public void GetAllIncludesSectionobject()
        {
            var arg = SampleDir.Lease197(out LeaseDTO lse);
            var sut = arg.MarketState.ActiveLeases;
            var all = sut.GetAll();
            foreach (var rec in all)
            {
                rec.Stall.Section.Should().NotBeNull();
                rec.Stall.Section.Name.Should().NotBeEmpty();
            }
        }
    }
}
