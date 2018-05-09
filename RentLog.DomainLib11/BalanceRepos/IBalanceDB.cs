using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IBalanceDB
    {
        DailyBillDTO  GetBill  (LeaseDTO lse, DateTime date);
    }
}
