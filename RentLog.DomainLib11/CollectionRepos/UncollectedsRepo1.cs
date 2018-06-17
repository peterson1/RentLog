using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class UncollectedsRepo1 : SimpleRepoShimBase<UncollectedLeaseDTO>, IUncollectedsRepo
    {
        private ITenantDBsDir _dir;
        private DateTime      _date;


        public UncollectedsRepo1(DateTime date, ISimpleRepo<UncollectedLeaseDTO> simpleRepo, ITenantDBsDir tenantDBsDir) : base(simpleRepo)
        {
            _dir  = tenantDBsDir;
            _date = date;
            //_dir.Collections.UnclosedDate(); <-- throws StackOverflow exception
        }


        public List<UncollectedLeaseDTO> InferUncollecteds(
            IEnumerable<IntendedColxnDTO> intendedColxns, 
            IEnumerable<UncollectedLeaseDTO> didNotOperate)
        {
            var asOfDate    = _dir.Collections.UnclosedDate();
            var intendedIDs = intendedColxns.Select(_ => _.Id).ToList();
            var noOpsIDs    = didNotOperate.Select(_ => _.Id).ToList();
            var actives     = GetUncollecteds(GetActiveLeases(), 
                                asOfDate, intendedIDs, noOpsIDs);
            var inactvs     = GetUncollecteds(GetInactiveLeases(),
                                asOfDate, intendedIDs, noOpsIDs);

            return actives.Concat(inactvs).ToList();
        }


        private List<UncollectedLeaseDTO> GetUncollecteds(
            List<LeaseDTO> leases, DateTime asOfDate,
            IEnumerable<int> intendedColxns, 
            IEnumerable<int> didNotOperate)
            => leases.Where (_ => IsUncollected(_, asOfDate, intendedColxns, didNotOperate))
                     .Select(_ => CreateUncollected(_))
                     .ToList();


        private bool IsUncollected(
            LeaseDTO lse, DateTime asOfDate,
            IEnumerable<int> intendedColxns, 
            IEnumerable<int> didNotOperate)
        {
            if (intendedColxns.Contains(lse.Id)) return false;
            if (didNotOperate.Contains(lse.Id)) return false;
            return lse.IsActive(asOfDate);
        }


        private BillAmounts GetTarget(LeaseDTO lse) => new BillAmounts
        {
            Rent     = GetDue(lse, BillCode.Rent),
            Rights   = GetDue(lse, BillCode.Rights),
            Electric = GetDue(lse, BillCode.Electric),
            Water    = GetDue(lse, BillCode.Water),
        };


        public decimal? GetDue(LeaseDTO lse, BillCode billCode)
        {
            var bill = _dir.Balances.GetBill(lse, _date).For(billCode);
            return _dir.DailyBiller.GetBillComposer(billCode).GetTotalDue(lse, bill, _date);
        }


        private UncollectedLeaseDTO CreateUncollected(LeaseDTO lse) => new UncollectedLeaseDTO
        {
            Lease   = lse,
            Targets = GetTarget(lse)
        };


        private List<LeaseDTO> GetActiveLeases()
            => _dir.MarketState.ActiveLeases.GetAll();


        private List<LeaseDTO> GetInactiveLeases()
            => _dir.MarketState
                   .InactiveLeases.GetAll()
                   .Select(_ => _ as LeaseDTO)
                   .ToList();
    }
}
    