using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public abstract class BillRowComposerBase : IBillRowComposer
    {
        private ICollectionsDir _colxnsDir;


        public BillRowComposerBase(ICollectionsDir collectionsDir)
        {
            _colxnsDir = collectionsDir;
        }

        protected abstract BillCode BillCode { get; }

        public abstract List<BillPenalty> ComputePenalties (LeaseDTO lse, DateTime date, decimal? previousBalance);
        protected abstract decimal        GetRegularDue    (LeaseDTO lse, BillState billState, DateTime date);


        public decimal TotalDue(LeaseDTO lse, BillState state, DateTime date)
            => (state.OpeningBalance ?? 0)
              + state.TotalPenalties
              + state.TotalAdjustments
              + GetRegularDue(lse, state, date);


        public decimal ComputeClosingBalance(LeaseDTO lse, BillState billState, DateTime date)
            => TotalDue(lse, billState, date) - billState.TotalPayments;


        public List<BillAdjustment> ReadAdjustments(LeaseDTO lse, DateTime date)
            => _colxnsDir.For(date).BalanceAdjs.GetAll()
                .Where(_ => _.LeaseId  == lse.Id
                         && _.BillCode == this.BillCode)
                .Select(_ => _.ToBillAdjustment()).ToList();


        public List<DailyBillDTO.BillPayment> ReadPayments(LeaseDTO lse, DateTime date)
        {
            var db      = _colxnsDir.For(date);
            var sec     = lse.Stall.Section;
            var fromCol = db.IntendedColxns[sec.Id].GetAll()
                            .Where(_ => _.Lease.Id == lse.Id
                                     && _.Actuals.For(this.BillCode).HasValue)
                            .Select(_ => ToPayment(_, db));

            var fromCas = db.CashierColxns.GetAll()
                            .Where(_ => _.Lease.Id == lse.Id
                                     && _.BillCode == this.BillCode)
                            .Select(_ => _.ToBillPayment());

            return fromCol.Concat(fromCas).ToList();
        }


        private BillPayment ToPayment(IntendedColxnDTO colxn, ICollectionsDB db) => new BillPayment
        {
            Amount    = colxn.Actuals.For(this.BillCode).Value,
            PRNumber  = colxn.PRNumber,
            Remarks   = colxn.Remarks,
            Collector = db.GetCollector(colxn.Lease),
        };
    }
}
