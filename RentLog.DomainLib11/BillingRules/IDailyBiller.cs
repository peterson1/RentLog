using RentLog.DomainLib11.DTOs;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public interface IDailyBiller
    {
        BillState ComputeBill(DailyBillDTO bill, BillCode billCode);
    }
}
