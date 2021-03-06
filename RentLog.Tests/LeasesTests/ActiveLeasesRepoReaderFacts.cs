﻿using FluentAssertions;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using Xunit;

namespace RentLog.Tests.LeasesTests
{
    [Trait("Active Leases Repo", "Sample Dir")]
    public class ActiveLeasesRepoReaderFacts
    {
        [Fact(DisplayName = "Find by ID includes Section object")]
        public void FindbyIDIncludesSectionobject()
        {
            var arg = SampleDir.Lease197();
            var sut = arg.MarketState.ActiveLeases;
            var rec = sut.Find(10, true);
            rec.Stall.Section.Should().NotBeNull();
            rec.Stall.Section.Name.Should().Be("WET");
        }


        [Fact(DisplayName = "GetAll includes Section object")]
        public void GetAllIncludesSectionobject()
        {
            var arg = SampleDir.Lease197();
            var sut = arg.MarketState.ActiveLeases;
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
            var arg = SampleDir.Nov29_BIG();
            var sut = arg.MarketState.ActiveLeases;
            var res = sut.BySection(2); //DRY section

            res.Count.Should().Be(71);
        }
    }
}
