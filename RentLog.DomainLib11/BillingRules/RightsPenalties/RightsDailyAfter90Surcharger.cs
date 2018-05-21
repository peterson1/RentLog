using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules.RightsPenalties
{
    public class RightsDailyAfter90Surcharger : IBillSurcharger
    {
        public const string RULE = "Daily Surcharge after 90 days";


        public List<BillPenalty> GetPenalties(LeaseDTO lse, DateTime date, decimal? oldBal)
        {
            //todo: apply Rights surcharge
            return null;
        }
    }
}
