using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public abstract class BillSurchargerBase : IBillSurcharger
    {
        public abstract string RuleName { get; }

        protected abstract List<BillPenalty> GetPenaltiesList(LeaseDTO lse, DateTime date, decimal? oldBal);
        protected abstract void ValidateRuleName(LeaseDTO lse);


        public List<BillPenalty> GetPenalties(LeaseDTO lse, DateTime date, decimal? oldBal)
        {
            ValidateRuleName(lse);
            return GetPenaltiesList(lse, date, oldBal);
        }
    }
}
