using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public abstract class BalanceDBBase : IBalanceDB
    {
        protected abstract IDailyBillsRepo GetRepo(LeaseDTO lse);


        public DailyBillDTO GetBill(LeaseDTO lse, DateTime date)
        {
            var repo = GetRepo(lse);
            var id   = date.ToBillID();
            return repo.Find(id, true);
        }
    }
}
