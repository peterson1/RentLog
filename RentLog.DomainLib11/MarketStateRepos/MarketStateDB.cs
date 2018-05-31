﻿using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public class MarketStateDB
    {
        public virtual string               DatabasePath    { get; set; }
        public virtual string               CurrentUser     { get; set; }
        public virtual IStallsRepo          Stalls          { get; set; }
        public virtual ISectionsRepo        Sections        { get; set; }
        public virtual ICollectorsRepo      Collectors      { get; set; }
        public virtual IActiveLeasesRepo    ActiveLeases    { get; set; }
        public virtual IInactiveLeasesRepo  InactiveLeases  { get; set; }
        public virtual IGLAccountsRepo      GLAccounts      { get; set; }
        public virtual string               SystemName      { get; set; }
        public virtual string               BranchName      { get; set; }

        public virtual IBalanceDB           Balances        { get; set; }
        public virtual ICollectionsDir      Collections     { get; set; }

        public LeaseDTO FindLease(int leaseID)
        {
            var match = ActiveLeases.Find(leaseID, false);
            if (match != null) return match;

            match = InactiveLeases.Find(leaseID, false);

            if (match == null)
                throw Fault.NoMatch<LeaseDTO>("Id", leaseID);

            return match;
        }


        public void RefreshStall(LeaseDTO lease)
        {
            if (lease == null) throw Fault.NullRef("Lease");
            if (lease.Stall == null) throw Fault.NullRef("Lease.Stall");
            lease.Stall = Stalls.Find(lease.Stall.Id, true);
        }
    }
}
