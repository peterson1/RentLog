using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IBalanceDBs
    {
        DailyBillDTO  GetBill  (LeaseDTO lse, DateTime date);
    }
}
