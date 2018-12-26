using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.MarketStateRepos
{
    public abstract class MarketStateDbBase : IMarketStateDB
    {
        private Dictionary<int, StallDTO> _stalls = new Dictionary<int, StallDTO>();

        public abstract int                 YearsBackCount  { get; set; }
        public virtual string               DatabasePath    { get; set; }
        public virtual string               CurrentUser     { get; set; }
        public virtual string               SystemName      { get; set; }
        public virtual string               BranchName      { get; set; }
        public virtual IStallsRepo          Stalls          { get; set; }
        public virtual ISectionsRepo        Sections        { get; set; }
        public virtual ICollectorsRepo      Collectors      { get; set; }
        public virtual IBankAccountsRepo    BankAccounts    { get; set; }
        public virtual IActiveLeasesRepo    ActiveLeases    { get; set; }
        public virtual IInactiveLeasesRepo  InactiveLeases  { get; set; }
        public virtual IGLAccountsRepo      GLAccounts      { get; set; }

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
            if (Stalls == null) return;
            if (lease == null) throw Fault.NullRef("Lease");
            if (lease.Stall == null) throw Fault.NullRef("Lease.Stall");

            //lease.Stall = Stalls.Find(lease.Stall.Id, true);
            var stallID = lease.Stall.Id;

            if (_stalls.TryGetValue(stallID, out StallDTO cached))
                lease.Stall = cached;
            else
            {
                lease.Stall = Stalls.Find(stallID, true);
                //_stalls[stallID] = lease.Stall;
                try
                {
                    _stalls?.Add(stallID, lease.Stall);
                }
                catch { }
            }
        }


        public List<LeaseDTO> ActiveLeasesFor(DateTime date)
        {
            if (date == DateTime.MinValue)
                return new List<LeaseDTO>();

            var activs = ActiveLeases.GetAll()
                            .Where(_ => _.IsActive(date));

            var inactvs = InactiveLeases.GetAll()
                            .Where(_ => _.IsActive(date));

            return activs.Concat(inactvs).ToList();
        }


        public List<LeaseDTO> GetAllLeases()
        {
            var activs  = ActiveLeases.GetAll();
            var inactvs = InactiveLeases.GetAll();
            return activs.Concat(inactvs).ToList();
        }


        public LeaseDTO GetOccupant(StallDTO stall)
        {
            var matches = ActiveLeases.GetAll()
                            .Where(_ => _.Stall.Id == stall.Id);

            if (!matches.Any()) return null;
            if (matches.Count() > 1)
                throw Bad.Data($"1 occupant for [{stall}] but found [{matches.Count()}]");
            return matches.Single();
        }


        public bool IsOccupied(StallDTO stall) => GetOccupant(stall) != null;
    }
}
