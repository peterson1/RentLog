using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.Tests.SampleDBs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RentLog.Tests.BillRowComposerTests
{
    [Trait("Bill Row Composer", "Sample DB Dir")]
    public class BillRowComposerBaseFacts
    {
        [Fact(DisplayName = "Reads Adjustments")]
        public void ReadsAdjustments()
        {
            var arg = SampleDir.HelenAblen_Dry8(out LeaseDTO lse);
            var sut = new SampleComposer(arg.Collections);

            sut._billCode = BillCode.Rent;
            var adj = sut.ReadAdjustments(lse, 4.May(2018));
            adj.Single().AmountOffset.Should().Be(-656);

            sut._billCode = BillCode.Rights;
            adj = sut.ReadAdjustments(lse, 4.May(2018));
            adj.Single().AmountOffset.Should().Be(-10_000);
        }


        [Fact(DisplayName = "Reads Payments")]
        public void ReadsPayments()
        {
            var arg = SampleDir.HelenAblen_Dry8(out LeaseDTO lse);
            var sut = new SampleComposer(arg.Collections);

            sut._billCode = BillCode.Rent;
            var pay = sut.ReadPayments(lse, 9.May(2018));
            pay.Single().Amount.Should().Be(160);

            sut._billCode = BillCode.Electric;
            pay = sut.ReadPayments(lse, 11.May(2018));
            pay.Single().Amount.Should().Be(33);
        }


        private class SampleComposer : BillRowComposerBase
        {
            public BillCode _billCode;
            protected override BillCode BillCode => _billCode;


            public SampleComposer(ICollectionsDir collectionsDir) : base(collectionsDir)
            {
            }


            public override decimal ComputeClosingBalance(DailyBillDTO.BillState billState)
            {
                throw new NotImplementedException();
            }

            public override List<DailyBillDTO.BillPenalty> ComputePenalties(LeaseDTO lse, DateTime date, decimal? previousBalance)
            {
                throw new NotImplementedException();
            }
        }
    }
}
