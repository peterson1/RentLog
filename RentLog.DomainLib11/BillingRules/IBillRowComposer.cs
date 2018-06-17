using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public interface IBillRowComposer
    {
        List<BillPayment>     ReadPayments           (LeaseDTO lse, DateTime date);
        List<BillAdjustment>  ReadAdjustments        (LeaseDTO lse, DateTime date);

        List<BillPenalty>     ComputePenalties       (LeaseDTO lse, DateTime date, decimal? previousBalance);
        decimal               ComputeClosingBalance  (LeaseDTO lse, BillState billState, DateTime date);
        decimal               GetTotalDue            (LeaseDTO lse, BillState state, DateTime date);
    }
}
