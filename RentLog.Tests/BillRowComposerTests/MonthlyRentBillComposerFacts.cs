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
    [Trait("Monthly Rent Bill Composer 1", "Solitary")]
    public class MonthlyRentBillComposerFacts
    {
        [Fact(DisplayName = "Rent due at billing day")]
        public void Rentdueatbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            var due = sut.GetTotalDue(lse, bil, 5.May(2018));
            due.Should().Be(1000);
        }


        [Fact(DisplayName = "Rent not due before billing day")]
        public void Rentnotduebeforebillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            var due = sut.GetTotalDue(lse, bil, 4.May(2018));
            due.Should().Be(0);
        }


        [Fact(DisplayName = "Rent not due after billing day")]
        public void Rentnotdueafterbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            var due = sut.GetTotalDue(lse, bil, 6.May(2018));
            due.Should().Be(0);
        }


        [Fact(DisplayName = "Due includes opening bal at billing day")]
        public void Dueincludesopeningbalatbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(2000);
            var due = sut.GetTotalDue(lse, bil, 5.May(2018));
            due.Should().Be(2000 + 1000);
        }


        [Fact(DisplayName = "Opening bal due before billing day")]
        public void Dueincludesopeningbalbeforebillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(2000);
            var due = sut.GetTotalDue(lse, bil, 4.June(2018));
            due.Should().Be(2000);
        }


        [Fact(DisplayName = "Opening bal due after billing day")]
        public void Dueincludesopeningbalafterbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(2000);
            var due = sut.GetTotalDue(lse, bil, 6.June(2018));
            due.Should().Be(2000);
        }


        [Fact(DisplayName = "Due includes penalties at billing day")]
        public void Dueincludespenaltyatbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            bil.Penalties[0].Amount = 456;
            var due = sut.GetTotalDue(lse, bil, 5.May(2018));
            due.Should().Be(1000 + 456);
        }


        [Fact(DisplayName = "Penalties due before billing day")]
        public void Penaltiesduebeforebillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            bil.Penalties[0].Amount = 456;
            var due = sut.GetTotalDue(lse, bil, 4.May(2018));
            due.Should().Be(456);
        }


        [Fact(DisplayName = "Penalties due after billing day")]
        public void Penaltiesdueafterbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            bil.Penalties[0].Amount = 456;
            var due = sut.GetTotalDue(lse, bil, 6.May(2018));
            due.Should().Be(456);
        }


        [Fact(DisplayName = "Due includes adjustments at billing day")]
        public void Dueincludesadjustmentsatbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            bil.Adjustments[0].AmountOffset = 456;
            var due = sut.GetTotalDue(lse, bil, 5.May(2018));
            due.Should().Be(1000 + 456);
        }


        [Fact(DisplayName = "Adjustments due before billing day")]
        public void Adjustmentsduebeforebillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            bil.Adjustments[0].AmountOffset = 456;
            var due = sut.GetTotalDue(lse, bil, 4.May(2018));
            due.Should().Be(456);
        }


        [Fact(DisplayName = "Adjustments due after billing day")]
        public void Adjustmentsdueafterbillingday()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            bil.Adjustments[0].AmountOffset = 456;
            var due = sut.GetTotalDue(lse, bil, 6.May(2018));
            due.Should().Be(456);
        }


        [Fact(DisplayName = "Zero Due if at Grace Period")]
        public void ZeroDueifatGracePeriod()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            var bil = SampleBillState(0);
            lse.Rent.GracePeriodDays = 5;

            sut.GetTotalDue(lse, bil, 1.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 2.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 3.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 4.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 5.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 6.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 7.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 8.May(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 4.June(2018)).Should().Be(0);
            sut.GetTotalDue(lse, bil, 5.June(2018)).Should().Be(1000);
        }


        [Fact(DisplayName = "Zero Due if inactive")]
        public void ZeroDueifinactive()
        {
            var sut = new RentBillComposer1(null);
            var lse = NewLease(5, 2.May(2018), 1000);
            lse.ContractEnd = 4.May(2018);
            var bil = SampleBillState(0);
            var due = sut.GetTotalDue(lse, bil, 5.May(2018));
            due.Should().Be(0);
        }


        private BillState SampleBillState(decimal? openingBal) => new BillState
        {
            OpeningBalance = openingBal,
            Penalties      = new List<BillPenalty>    { new BillPenalty   () },
            Adjustments    = new List<BillAdjustment> { new BillAdjustment() },
        };


        private LeaseDTO NewLease(decimal billDay, DateTime startDate, decimal regularRate) => new LeaseDTO
        {
            ContractStart = startDate,
            ContractEnd   = startDate.AddYears(1),
            Stall         = new StallDTO { IsOperational = true },
            Rent          = new RentParams
            {
                Interval     = BillInterval.Monthly,
                RegularRate  = regularRate,
                PenaltyRule  = RentPenalty.MonthlySurcharge,
                PenaltyRate1 = 0.03M,
                PenaltyRate2 = billDay,
            },
        };
    }
}
