using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.LeasesTests
{
    [Trait("Active Leases Repo", "Sample Dir")]
    public class InactiveLeasesRepoReaderFacts
    {
        [Fact(DisplayName = "Find by ID includes Section object")]
        public void FindbyIDIncludesSectionobject()
        {
            var arg = SampleDir.Aug27_GRY();
            var sut = arg.MarketState.InactiveLeases;
            var rec = sut.Find(16, true);
            rec.Stall.Section.Should().NotBeNull();
            rec.Stall.Section.Name.Should().Be("DRY");
        }


        [Fact(DisplayName = "GetAll includes Section object")]
        public void GetAllIncludesSectionobject()
        {
            var arg = SampleDir.Aug27_GRY();
            var sut = arg.MarketState.InactiveLeases;
            var all = sut.GetAll();
            foreach (var rec in all)
            {
                rec.Stall.Section.Should().NotBeNull();
                rec.Stall.Section.Name.Should().NotBeEmpty();
            }
        }


        [Fact(DisplayName = "BySection filters by SecID")]
        public void BySectionfiltersbySecID()
        {
            var arg = SampleDir.Aug27_GRY();
            var sut = arg.MarketState.InactiveLeases;
            var res = sut.BySection(2); //DRY section

            res.Count.Should().Be(65);
        }
    }
}
