using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.FileSystemTools;
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
        private ITenantDBsDir _dir;
        private DateTime      _date;
        private SectionDTO    _sec;
        private Dictionary<int, DailyBillDTO> _soaRowsByLseID;
        private IPersistentCollection<LeaseDTO> _cache;


        public UncollectedsRepo1(IPersistentCollection<LeaseDTO> persistentCollection, SectionDTO sectionDTO, DateTime date, ISimpleRepo<UncollectedLeaseDTO> simpleRepo, ITenantDBsDir tenantDBsDir) : base(simpleRepo)
        {
            _sec  = sectionDTO;
            _dir  = tenantDBsDir;
            _date = date;
            _cache = persistentCollection;
            _cache.Clear();
            //_dir.Collections.UnclosedDate();// <-- throws StackOverflow exception
        }


        private Dictionary<int, DailyBillDTO> CreateSoaRowsDictionary()
        {
            var dict = new Dictionary<int, DailyBillDTO>();

            foreach (var lse in GetActiveLeases())
                dict.Add(lse.Id, _dir.Balances.GetBill(lse, _date));

            return dict;
        }


        public List<UncollectedLeaseDTO> InferUncollecteds(
            IEnumerable<IntendedColxnDTO> intendedColxns, 
            IEnumerable<UncollectedLeaseDTO> didNotOperate)
        {
            var asOfDate    = _dir.Collections.UnclosedDate();
            var intendedIDs = intendedColxns.Select(_ => _.Lease.Id).ToList();
            var noOpsIDs    = didNotOperate.Select(_ => _.Lease.Id).ToList();

            if (_soaRowsByLseID == null)
                _soaRowsByLseID = CreateSoaRowsDictionary();

            var actives     = GetUncollecteds(GetActiveLeases(), 
                                asOfDate, intendedIDs, noOpsIDs);
            var inactvs     = GetUncollecteds(GetInactiveLeases(),
                                asOfDate, intendedIDs, noOpsIDs);

            return actives.Concat(inactvs)
                          .OrderBy(_ => _.Lease.Stall.Name)
                          .ToList();
        }


        private List<UncollectedLeaseDTO> GetUncollecteds(
            IEnumerable<LeaseDTO> leases, DateTime asOfDate,
            IEnumerable<int> intendedColxns,
            IEnumerable<int> didNotOperate)
        {
            var uncolLses = GetUncollectedLeases(leases, asOfDate, intendedColxns, didNotOperate);
            var list      = new List<UncollectedLeaseDTO>();

            foreach (var lse in uncolLses)
                list.Add(CreateUncollected(lse));

            return list;
        }


        private List<LeaseDTO> GetUncollectedLeases(IEnumerable<LeaseDTO> leases, DateTime asOfDate, IEnumerable<int> intendedColxns, IEnumerable<int> didNotOperate) 
            => leases.Where(_ => IsUncollected(_, asOfDate, intendedColxns, didNotOperate)).ToList();


        private bool IsUncollected(
            LeaseDTO lse, DateTime asOfDate,
            IEnumerable<int> intendedColxns, 
            IEnumerable<int> didNotOperate)
        {
            if (lse.Stall.Section.Id != _sec.Id) return false;
            if (didNotOperate.Contains(lse.Id)) return false;
            if (intendedColxns.Contains(lse.Id)) return false;
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
            if (_soaRowsByLseID == null)
                _soaRowsByLseID = CreateSoaRowsDictionary();

            if (!_soaRowsByLseID.TryGetValue(lse.Id, out DailyBillDTO row))
                row = _dir.Balances.GetBill(lse, _date);
            
            var bill = row?.For(billCode);
            return _dir.DailyBiller.GetBillComposer(billCode).GetTotalDue(lse, bill, _date);
        }


        private UncollectedLeaseDTO CreateUncollected(LeaseDTO lse) => new UncollectedLeaseDTO
        {
            Lease         = lse,
            StallSnapshot = lse.Stall,
            Targets       = GetTarget(lse)
        };


        private List<LeaseDTO> GetActiveLeases()
            => _dir.MarketState.ActiveLeases.BySection(_sec.Id);


        private IEnumerable<LeaseDTO> GetInactiveLeases()
        {
            //return _dir.MarketState.InactiveLeases.BySection(_sec.Id)
            //           .Select(_ => _ as LeaseDTO).ToList();


            //if (_cacheKey == null)
            //    _cacheKey = $"{DateTime.Now.Ticks}_{_sec.Id}";

            //if (_disk.TryGet(_cacheKey, out List<LeaseDTO> list))
            //    return list;

            //list = _dir.MarketState
            //           .InactiveLeases.BySection(_sec.Id)
            //           .Select(_ => _ as LeaseDTO)
            //           .ToList();

            //_disk.Put(_cacheKey, list);
            //return list;



            if (_cache.Any()) return _cache.Enumerate();

            var list = _dir.MarketState
                           .InactiveLeases.BySection(_sec.Id)
                           .Select(_ => _ as LeaseDTO);
            _cache.Set(list);
            return list;
        }
    }
}
    