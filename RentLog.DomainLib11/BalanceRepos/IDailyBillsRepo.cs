using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public interface IDailyBillsRepo : ISimpleRepo<DailyBillDTO>
    {
        void UpdateFrom(DateTime date);
        //void UpdateFrom(DateTime date, BillCode billCode, IDailyBiller dailyBiller);
        void UpdateFrom(DateTime date, BillCode billCode);
    }
}
