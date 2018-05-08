using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IBalanceDBs
    {
        DateTime      LastClosedDate { get; }
        DailyBillDTO  GetLatest      (LeaseDTO lse);
    }
}
