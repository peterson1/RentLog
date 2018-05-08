using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.BalanceRepos
{
    public abstract class BalanceDBsBase : IBalanceDBs
    {
        public BalanceDBsBase()
        {
            LastClosedDate = GetLastClosedDate();
        }


        public DateTime LastClosedDate { get; }


        protected abstract DateTime        GetLastClosedDate ();
        protected abstract IDailyBillsRepo GetRepo           (LeaseDTO lse);


        public DailyBillDTO GetLatest(LeaseDTO lse)
        {
            var repo = GetRepo(lse);
            throw new NotImplementedException();
        }

    }
}
