using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules.RightsPenalties;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Linq;
using Xunit;

namespace RentLog.Tests.SurchargerTests.RightsSurchargerTests
{
    [Trait("Rights Surcharge", "Solitary")]
    public class RightsDailyAfter90SurchargerFacts
    {
        [Fact(DisplayName = "1 day before due")]
        public void TestMethod00001()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(7.May(2018));
            var oldBal  = 100;
            var charges = sut.GetPenalties(lse, 6.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "At due date")]
        public void TestMethod00002()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(7.May(2018));
            var oldBal  = 100;
            var charges = sut.GetPenalties(lse, 7.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "1 day after due")]
        public void TestMethod00003()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(7.May(2018));
            var oldBal  = 100;
            var charges = sut.GetPenalties(lse, 8.May(2018), oldBal);
            charges.Should().HaveCount(1);
            charges.Single().Amount.Should().Be(oldBal * 0.2M);
        }


        [Fact(DisplayName = "2 days after due")]
        public void TestMethod00004()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(7.May(2018));
            var oldBal  = 120;
            var charges = sut.GetPenalties(lse, 9.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "89 days after due")]
        public void TestMethod00005()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var due = 7.May(2018);
            var dte = due.AddDays(89);
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(due);
            var oldBal  = 120;
            var charges = sut.GetPenalties(lse, dte, oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "90 days after due")]
        public void TestMethod00006()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var due = 7.May(2018);
            var dte = due.AddDays(90);
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(due);
            var oldBal  = 120M;
            var expctd  = Math.Round(oldBal * 0.03M, 0);
            var charges = sut.GetPenalties(lse, dte, oldBal);
            charges.Should().HaveCount(1);
            charges.Single().Amount.Should().Be(expctd);
        }


        [Fact(DisplayName = "91 days after due")]
        public void TestMethod00007()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var due = 7.May(2018);
            var dte = due.AddDays(91);
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(due);
            var oldBal  = 123.60M;
            var expctd  = Math.Round(oldBal * 0.03M, 0);
            var charges = sut.GetPenalties(lse, dte, oldBal);
            charges.Should().HaveCount(1);
            charges.Single().Amount.Should().Be(expctd);
        }


        [Fact(DisplayName = "92 days after due")]
        public void TestMethod00008()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var due = 7.May(2018);
            var dte = due.AddDays(92);
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M)
                               .RightsDueOn(due);
            var oldBal  = 127.308M;
            var expctd  = Math.Round(oldBal * 0.03M, 0);
            var charges = sut.GetPenalties(lse, dte, oldBal);
            charges.Should().HaveCount(1);
            charges.Single().Amount.Should().Be(expctd);
        }


        [Fact(DisplayName = "Error if Different Rule")]
        public void ErrorifDifferentRule()
        {
            var sut = new RightsDailyAfter90Surcharger();
            var lse = LeaseWithPenaltyRates(0.2M, 0.03M);
            lse.Rights.PenaltyRule = "a different rule";

            sut.Invoking(_ => _.GetPenalties(lse, 3.May(2018), 123))
                .Should().Throw<BadKeyException>();
        }


        [Fact(DisplayName = "Null if inactive lease")]
        public void Nullifinactivelease()
        {
            var sut     = new RightsDailyAfter90Surcharger();
            var lse     = new InactiveLeaseDTO { Rights = new RightsParams { PenaltyRule = sut.RuleName } };
            var charges = sut.GetPenalties(lse, 3.May(2018), 123);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "Null if negative balance")]
        public void Nullifnegativebalance()
        {
            var sut     = new RightsDailyAfter90Surcharger();
            var lse     = LeaseWithPenaltyRates(0.2M, 0.03M);
            var oldBal  = -123;
            var charges = sut.GetPenalties(lse, 3.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "Null if zero balance")]
        public void Nullifzerobalance()
        {
            var sut     = new RightsDailyAfter90Surcharger();
            var lse     = LeaseWithPenaltyRates(0.2M, 0.03M);
            var oldBal  = 0;
            var charges = sut.GetPenalties(lse, 3.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "Null if null balance")]
        public void Nullifnullbalance()
        {
            var sut     = new RightsDailyAfter90Surcharger();
            var lse     = LeaseWithPenaltyRates(0.2M, 0.03M);
            var oldBal  = (decimal?)null;
            var charges = sut.GetPenalties(lse, 3.May(2018), oldBal);
            charges.Should().BeNull();
        }


        private LeaseDTO LeaseWithPenaltyRates(decimal penaltyRate1, decimal penaltyRate2) => new LeaseDTO
        {
            ContractStart = 2.May(2018),
            ContractEnd   = 2.May(2019),
            Stall         = new StallDTO { IsOperational = true },
            Rights        = new RightsParams
            {
                PenaltyRule    = new RightsDailyAfter90Surcharger().RuleName,
                PenaltyRate1   = penaltyRate1, 
                PenaltyRate2   = penaltyRate2,
            }
        };
    }

    internal static class RightsDailyAfter90SurchargerFactsExtensions
    {
        internal static LeaseDTO RightsDueOn(this LeaseDTO lse, DateTime rightsDueDate)
        {
            lse.Rights.SettlementDays = (int)(rightsDueDate - lse.ContractStart).TotalDays;
            return lse;
        }
    }
}
