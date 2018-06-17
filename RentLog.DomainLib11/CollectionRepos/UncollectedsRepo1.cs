using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class UncollectedsRepo1 : SimpleRepoShimBase<UncollectedLeaseDTO>, IUncollectedsRepo
    {
        //private ITenantDBsDir _dir;
        //private DateTime      _asOfDate;


        public UncollectedsRepo1(ISimpleRepo<UncollectedLeaseDTO> simpleRepo) : base(simpleRepo)
        {
            //_dir      = tenantDBsDir;
            //_asOfDate = _dir.Collections.UnclosedDate();
        }


        public List<UncollectedLeaseDTO> InferUncollecteds(
            IEnumerable<IntendedColxnDTO> intendedColxns, 
            IEnumerable<UncollectedLeaseDTO> didNotOperate)
        {
            //var actives = GetUncollecteds(GetActiveLeases(), 
            //                intendedColxns, didNotOperate);
            //var inactvs = GetUncollecteds(GetInactiveLeases(), 
            //                intendedColxns, didNotOperate);

            //return actives.Concat(inactvs).ToList();
            throw new NotImplementedException();
        }


        private List<UncollectedLeaseDTO> GetUncollecteds(List<LeaseDTO> leases, IEnumerable<IntendedColxnDTO> intendedColxns, IEnumerable<UncollectedLeaseDTO> didNotOperate)
            => leases.Where (_ => IsUncollected(_, intendedColxns, didNotOperate))
                     .Select(_ => CreateUncollected(_))
                     .ToList();


        private bool IsUncollected(LeaseDTO lse, IEnumerable<IntendedColxnDTO> intendedColxns, IEnumerable<UncollectedLeaseDTO> didNotOperate)
        {
            //if (intendedColxns.Select(_ => 
            //    _.Lease.Id).Contains(lse.Id)) return false;

            //if (didNotOperate.Select(_ =>
            //    _.Lease.Id).Contains(lse.Id)) return false;

            //return lse.IsActive(_asOfDate);
            throw new NotImplementedException();
        }


        private BillAmounts GetTarget(LeaseDTO lse) => new BillAmounts
        {
            Rent     = GetDue(lse, BillCode.Rent),
            Rights   = GetDue(lse, BillCode.Rights),
            Electric = GetDue(lse, BillCode.Electric),
            Water    = GetDue(lse, BillCode.Water),
        };


        //todo: test this directly using sample DBs
        private decimal? GetDue(LeaseDTO lse, BillCode billCode)
        {
            //var bill = _dir.Balances.GetBill(lse, _asOfDate).For(billCode);
            //return _dir.DailyBiller.GetBillComposer(billCode).GetTotalDue(lse, bill, _asOfDate);
            throw new NotImplementedException();
        }


        private UncollectedLeaseDTO CreateUncollected(LeaseDTO lse) => new UncollectedLeaseDTO
        {
            Lease   = lse,
            Targets = GetTarget(lse)
        };


        //private List<LeaseDTO> GetActiveLeases()
        //    => _dir.MarketState
        //           .ActiveLeases.GetAll()
        //           .Where(_ => _.IsActive(_asOfDate))
        //           .ToList();


        //private List<LeaseDTO> GetInactiveLeases()
        //    => _dir.MarketState
        //           .InactiveLeases.GetAll()
        //           .Where (_ => _.IsActive(_asOfDate))
        //           .Select(_ => _ as LeaseDTO)
        //           .ToList();
    }
}
    