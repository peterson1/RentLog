using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public abstract class BalanceDBBase : IBalanceDB
    {
        public abstract IDailyBillsRepo GetRepo(int leaseID);

        public IDailyBillsRepo GetRepo(LeaseDTO lse) => GetRepo(lse.Id);


        public DailyBillDTO GetBill(LeaseDTO lse, DateTime date)
        {
            var repo = GetRepo(lse);
            var id   = date.ToBillID();
            return repo.Find(id, true);
        }
    }
}
