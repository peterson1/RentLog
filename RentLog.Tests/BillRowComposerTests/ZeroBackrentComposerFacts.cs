using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using Xunit;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.Tests.BillRowComposerTests
{
    [Trait("Zero Backrent Bill Composer", "Solitary")]
    public class ZeroBackrentComposerFacts
    {
        [Fact(DisplayName = "Due includes rent rate")]
        public void Dueincludesrentrate()
        {
            var sut = new RentBillComposer1(null);
            var lse = SampleLease(out DateTime date);
            var bil = SampleBillState();
            var due = sut.GetTotalDue(lse, bil, date);
            due.Should().Be(lse.Rent.RegularRate);
            var end = sut.ComputeClosingBalance(lse, bil, date);
            end.Should().Be(0);
        }


        [Fact(DisplayName = "Due includes opening bal")]
        public void Dueincludesopeningbal()
        {
            var sut = new RentBillComposer1(null);
            var lse = SampleLease(out DateTime date);
            var bil = SampleBillState();
            bil.OpeningBalance = 123;
            var due = sut.GetTotalDue(lse, bil, date);
            due.Should().Be(bil.OpeningBalance + lse.Rent.RegularRate);
            var end = sut.ComputeClosingBalance(lse, bil, date);
            end.Should().Be(0);
        }


        [Fact(DisplayName = "Due includes penalties")]
        public void Dueincludespenalty()
        {
            var sut = new RentBillComposer1(null);
            var lse = SampleLease(out DateTime date);
            var bil = SampleBillState();
            bil.Penalties[0].Amount = 456;
            var due = sut.GetTotalDue(lse, bil, date);
            due.Should().Be(bil.TotalPenalties + lse.Rent.RegularRate);
            var end = sut.ComputeClosingBalance(lse, bil, date);
            end.Should().Be(0);
        }


        [Fact(DisplayName = "Due includes adjustments")]
        public void Dueincludesadjustments()
        {
            var sut = new RentBillComposer1(null);
            var lse = SampleLease(out DateTime date);
            var bil = SampleBillState();
            bil.Adjustments[0].AmountOffset = 789;
            var due = sut.GetTotalDue(lse, bil, date);
            due.Should().Be(bil.TotalAdjustments + lse.Rent.RegularRate);
            var end = sut.ComputeClosingBalance(lse, bil, date);
            end.Should().Be(0);
        }


        [Fact(DisplayName = "Zero Due if at Grace Period")]
        public void ZeroDueifatGracePeriod()
        {
            var sut = new RentBillComposer1(null);
            var bil = SampleBillState();
            var lse = SampleLease(out DateTime date);
            lse.Rent.GracePeriodDays = 3;

            sut.GetTotalDue(lse, bil, 1.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 2.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 3.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 4.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 5.May(2018)).Should().Be(120);
            sut.ComputeClosingBalance(lse, bil, 5.May(2018)).Should().Be(0);
        }


        [Fact(DisplayName = "Zero Due if inactive")]
        public void ZeroDueifinactive()
        {
            var sut = new RentBillComposer1(null);
            var lse = SampleLease(out DateTime date);
            lse.ContractEnd = 2.May(2018);
            var bil = SampleBillState();
            var due = sut.GetTotalDue(lse, bil, 3.May(2018));
            due.Should().Be(0);
            var end = sut.ComputeClosingBalance(lse, bil, 3.May(2018));
            end.Should().Be(0);
        }


        private BillState SampleBillState() => new BillState
        {
            Penalties   = new List<BillPenalty>    { new BillPenalty   () },
            Adjustments = new List<BillAdjustment> { new BillAdjustment() },
        };


        private LeaseDTO SampleLease(out DateTime activeDate)
        {
            activeDate = 15.May(2018);
            return new LeaseDTO
            {
                ContractStart = 2.May(2018),
                ContractEnd   = 29.May(2018),
                Rent          = new RentParams
                {
                    Interval     = BillInterval.Daily,
                    RegularRate  = 120,
                    PenaltyRule  = RentPenalty.ZeroBackrent,
                    PenaltyRate1 = 0.03M,
                },
                Stall = new StallDTO { IsOperational = true }
            };
        }
    }
}
