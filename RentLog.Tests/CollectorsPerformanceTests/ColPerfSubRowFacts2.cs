using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using Xunit;

namespace RentLog.Tests.CollectorsPerformanceTests
{
    [Trait("ColPerf Sub Row", "Solitary")]
    public class ColPerfSubRowFacts2
    {
        [Fact(DisplayName = "Rights Target: zero bal")]
        public void TestMethod00001()
        {
            var bal = 0;
            var dte = 27.May(2018);
            var lse = new LeaseDTO();
            CollectorPerfSubRow.GetRightsTarget(bal, dte, lse)
                .Should().Be(0);
        }


        [Fact(DisplayName = "Rights Target: negative bal")]
        public void TestMethod00002()
        {
            var bal = -100;
            var dte = 27.May(2018);
            var lse = new LeaseDTO();
            CollectorPerfSubRow.GetRightsTarget(bal, dte, lse)
                .Should().Be(0);
        }


        [Fact(DisplayName = "Rights Target: at due date")]
        public void TestMethod00003()
        {
            var bal = 123;
            var dte = 27.May(2018);
            var lse = LeaseWithRightsDue(dte);
            CollectorPerfSubRow.GetRightsTarget(bal, dte, lse)
                .Should().Be(bal);
        }


        [Fact(DisplayName = "Rights Target: after due date")]
        public void TestMethod00004()
        {
            var bal = 123;
            var dte = 27.May(2018);
            var lse = LeaseWithRightsDue(25.May(2018));
            CollectorPerfSubRow.GetRightsTarget(bal, dte, lse)
                .Should().Be(bal);
        }


        [Fact(DisplayName = "Rights Target: before due date")]
        public void TestMethod00005()
        {
            var bal = 123;
            var dte = 27.May(2018);
            var lse = LeaseWithRightsDue(30.May(2018));
            CollectorPerfSubRow.GetRightsTarget(bal, dte, lse)
                .Should().Be(bal / 3);
        }


        private LeaseDTO LeaseWithRightsDue(DateTime rightsDueDate)
        {
            var rights = new RightsParams
            {
                SettlementDays = 180
            };
            return new LeaseDTO
            {
                ContractStart = rightsDueDate.AddDays(rights.SettlementDays * -1),
                Rights        = rights
            };
        }
    }
}
