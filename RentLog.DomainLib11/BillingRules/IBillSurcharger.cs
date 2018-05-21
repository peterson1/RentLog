using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public interface IBillSurcharger
    {
        List<BillPenalty> GetPenalties(LeaseDTO lse, DateTime date, decimal? oldBal);
    }
}
