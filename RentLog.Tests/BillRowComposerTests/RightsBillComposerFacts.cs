using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using Xunit;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.Tests.BillRowComposerTests
{
    [Trait("Rights Bill Composer 1", "Solitary")]
    public class RightsBillComposerFacts
    {
        [Fact(DisplayName = "Total Amount due at contract start")]
        public void TotalAmountdueatcontractstart()
        {
            var lse = SampleLease();
            var bil = SampleBillState();
            var sut = new RightsBillComposer1(null);

            sut.TotalDue(lse, bil, 1.May(2018)).Should().Be(0);
            sut.TotalDue(lse, bil, 2.May(2018)).Should().Be(lse.Rights.TotalAmount);
            sut.TotalDue(lse, bil, 3.May(2018)).Should().Be(0);
        }


        private LeaseDTO SampleLease() => new LeaseDTO
        {
            ContractStart = 2.May(2018),
            ContractEnd   = 29.May(2018),
            Rights        = new RightsParams
            {
                TotalAmount = 10_000
            }
        };


        private BillState SampleBillState() => new BillState
        {
            Penalties   = new List<BillPenalty>    { new BillPenalty   () },
            Adjustments = new List<BillAdjustment> { new BillAdjustment() },
        };
    }
}
