using FluentAssertions;
using FluentAssertions.Extensions;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.Tests.SampleDBs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.Tests.BillRowComposerTests
{
    [Trait("Bill Composer Base", "Sample DB Dir")]
    public class BillRowComposerBaseFacts
    {
        [Fact(DisplayName = "Reads Adjustments")]
        public void ReadsAdjustments()
        {
            var arg = SampleDir.Lease197();
            var lse = arg.MarketState.ActiveLeases.Find(197, true);
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
            var arg = SampleDir.Lease197();
            var lse = arg.MarketState.ActiveLeases.Find(197, true);
            var sut = new SampleComposer(arg.Collections);

            sut._billCode = BillCode.Rent;
            var pay = sut.ReadPayments(lse, 9.May(2018));
            pay.Single().Amount.Should().Be(160);

            sut._billCode = BillCode.Electric;
            pay = sut.ReadPayments(lse, 11.May(2018));
            pay.Single().Amount.Should().Be(33);
            pay.Single().Collector.Name.Should().Be("Jomar Pasaludos");
        }


        private class SampleComposer : BillRowComposerBase
        {
            public BillCode _billCode;
            protected override BillCode BillCode => _billCode;


            public SampleComposer(ICollectionsDir collectionsDir) : base(collectionsDir)
            {
            }


            public override List<DailyBillDTO.BillPenalty> ComputePenalties(LeaseDTO lse, DateTime date, decimal? previousBalance)
            {
                throw new NotImplementedException();
            }


            protected override decimal GetRegularDue(LeaseDTO lse, BillState billState, DateTime date)
            {
                throw new NotImplementedException();
            }
        }
    }
}
