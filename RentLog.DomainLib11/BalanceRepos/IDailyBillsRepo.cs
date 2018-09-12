using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IDailyBillsRepo : ISimpleRepo<DailyBillDTO>
    {
        void RecomputeFrom      (DateTime date);
        void RecomputeAll       ();
        void ProcessBalancedDay (DateTime balancedDay);
    }
}
