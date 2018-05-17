using RentLog.DomainLib11.DTOs;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public interface IDailyBiller
    {
        BillState ComputeBill(BillCode billCode, LeaseDTO lease, DateTime date, decimal? previousBalance);
    }
}
