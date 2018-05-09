using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IBalanceDB
    {
        IDailyBillsRepo  GetRepo  (LeaseDTO lse);
        DailyBillDTO     GetBill  (LeaseDTO lse, DateTime date);
    }
}
