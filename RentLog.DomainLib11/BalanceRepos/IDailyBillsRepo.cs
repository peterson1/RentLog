using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IDailyBillsRepo : ISimpleRepo<DailyBillDTO>
    {
        void UpdateFrom(DateTime date, BillCode billCode);
    }
}
