using CommonTools.Lib11.ExceptionTools;
using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Linq;
using Xunit;

namespace RentLog.Tests.SurchargerTests.RentSurchargerTests
{
    [Trait("Rent Daily Surcharge", "Solitary")]
    public class RentDailySurchargeFacts
    {
        [Fact(DisplayName = "Rate * Balance")]
        public void RateTimesBalance()
        {
            var sut     = new RentDailySurcharger();
            var lse     = LeaseWithPenaltyRate(0.03M);
            var date    = 3.May(2018);
            var oldBal  = 100;
            var charges = sut.GetPenalties(lse, date, oldBal);
            charges.Should().HaveCount(1);
            var penalty = charges.Single();
            penalty.Amount.Should().Be(3);
            penalty.Label.Should().Be(sut.RuleName);
        }


        [Fact(DisplayName = "Rate * Balance (rounded)")]
        public void RateTimesBalancerounded()
        {
            var sut     = new RentDailySurcharger();
            var lse     = LeaseWithPenaltyRate(0.03M);
            var date    = 3.May(2018);
            var oldBal  = 90;
            var charges = sut.GetPenalties(lse, date, oldBal);
            charges.Should().HaveCount(1);
            var penalty = charges.Single();
            penalty.Amount.Should().Be(3);
            penalty.Label.Should().Be(sut.RuleName);
        }


        [Fact(DisplayName = "Error if Different Rule")]
        public void ErrorifDifferentRule()
        {
            var sut = new RentDailySurcharger();
            var lse = LeaseWithPenaltyRate(0.03M);
            lse.Rent.PenaltyRule = "a different rule";

            sut.Invoking(_ => _.GetPenalties(lse, 3.May(2018), 123))
                .Should().Throw<BadKeyException>();
        }


        [Fact(DisplayName = "Null if inactive lease")]
        public void Nullifinactivelease()
        {
            var sut     = new RentDailySurcharger();
            var lse     = new InactiveLeaseDTO { Rent = new RentParams { PenaltyRule = sut.RuleName } };
            var charges = sut.GetPenalties(lse, 3.May(2018), 123);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "Null if negative balance")]
        public void Nullifnegativebalance()
        {
            var sut     = new RentDailySurcharger();
            var lse     = LeaseWithPenaltyRate(0.03M);
            var oldBal  = -123;
            var charges = sut.GetPenalties(lse, 3.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "Null if zero balance")]
        public void Nullifzerobalance()
        {
            var sut     = new RentDailySurcharger();
            var lse     = LeaseWithPenaltyRate(0.03M);
            var oldBal  = 0;
            var charges = sut.GetPenalties(lse, 3.May(2018), oldBal);
            charges.Should().BeNull();
        }


        [Fact(DisplayName = "Null if null balance")]
        public void Nullifnullbalance()
        {
            var sut     = new RentDailySurcharger();
            var lse     = LeaseWithPenaltyRate(0.03M);
            var oldBal  = (decimal?)null;
            var charges = sut.GetPenalties(lse, 3.May(2018), oldBal);
            charges.Should().BeNull();
        }


        private LeaseDTO LeaseWithPenaltyRate(decimal penaltyRate1) => new LeaseDTO
        {
            ContractStart = 2.May(2018),
            ContractEnd   = 29.May(2018),
            Stall         = new StallDTO { IsOperational = true },
            Rent          = new RentParams
            {
                Interval     = BillInterval.Daily,
                PenaltyRule  = new RentDailySurcharger().RuleName,
                PenaltyRate1 = penaltyRate1
            }
        };
    }
}
