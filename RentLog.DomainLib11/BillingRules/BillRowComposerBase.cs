﻿using RentLog.DomainLib11.CollectionRepos;
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
        protected abstract decimal        GetRegularDue    (LeaseDTO lse, DateTime date);


        public decimal GetTotalDue(LeaseDTO lse, BillState state, DateTime date)
        {
            if (state == null)
                return GetRegularDue(lse, date);
            else
                return (state.OpeningBalance ?? 0)
                      + state.TotalPenalties
                      + state.TotalAdjustments
                      + GetRegularDue(lse, date);
        }

        public virtual decimal ComputeClosingBalance(LeaseDTO lse, BillState billState, DateTime date)
            => GetTotalDue(lse, billState, date) - billState.TotalPayments;


        public List<BillAdjustment> ReadAdjustments(LeaseDTO lse, DateTime date)
        {
            var db = _colxnsDir.For(date);
            if (db == null) return new List<BillAdjustment>();
            return db.BalanceAdjs.GetAll()
                .Where(_ => _.LeaseId  == lse.Id
                         && _.BillCode == this.BillCode)
                .Select(_ => _.ToBillAdjustment()).ToList();
        }


        public List<DailyBillDTO.BillPayment> ReadPayments(LeaseDTO lse, DateTime date)
        {
            var db = _colxnsDir.For(date);
            if (db == null) return new List<BillPayment>();
            var sec = lse.Stall.Section;

            var fromCol = db.IntendedColxns[sec.Id].GetAll()
                            .Where(_ => _.Lease.Id == lse.Id
                                && IsValidValue(_))
                            .Select(_ => ToPayment(_, db));

            var fromCas = db.CashierColxns.GetAll()
                            .Where(_ => _.Lease.Id == lse.Id
                                     && _.BillCode == this.BillCode)
                            .Select(_ => _.ToBillPayment());

            return fromCol.Concat(fromCas).ToList();
        }


        private bool IsValidValue(IntendedColxnDTO colxn)
        {
            var val = colxn.Actuals.For(this.BillCode);
            if (!val.HasValue) return false;
            return val.Value != 0M;
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
