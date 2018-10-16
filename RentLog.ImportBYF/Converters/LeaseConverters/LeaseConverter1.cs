using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public class LeaseConverter1 : ComparisonsListBase
    {
        private List<StallDTO> _stalls;
        //private string         _cacheDir;
        private IDictionary<long, DateTime> _terminations;
        private DateTime _lastClosedDate;


        public LeaseConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf    = Cast(byfRecord);
            var stall  = _stalls?.Single(_ => _.Id == byf.Stall.Id.Value);
            var active = new LeaseDTO
            {
                Id                   = (int)byf.Id.Value,
                Tenant               = byf.Tenant.CastTenant(),
                Stall                = stall,
                ContractStart        = byf.ContractStart.ToLocalTime(),
                ContractEnd          = byf.ContractEnd.ToLocalTime(),
                ProductToSell        = byf.ProductToSell.Trim().NullIfBlank(),
                Rent                 = byf.CastRentParams(),
                Rights               = byf.CastRightsParams(),
                ApplicationSubmitted = byf.ApplicationSubmitted,
                Remarks              = byf.Remarks.Trim().NullIfBlank(),
            };

            //if (_terminations == null) return active;

            //if (_terminations.TryGetValue(byf.Id.Value, out DateTime termDate))
            //    return new InactiveLeaseDTO(active, "Terminated", termDate, "Migrator");
            //else
            //    return active;

            return active.AsInactiveIfSo(_terminations, _lastClosedDate);
        }


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir appArgs)
        {
            var mkt     = appArgs.MarketState;
            _stalls     = mkt.Stalls.GetAll();
            var actives = mkt.ActiveLeases.GetAll()
                             .Select(_ => _ as IDocumentDTO);
            var inactvs = mkt.InactiveLeases.GetAll()
                             .Select(_ => _ as IDocumentDTO);
            return actives.Concat(inactvs).ToList();
        }


        public override List<object> GetListFromBYF(string cacheDir)
        {
            _terminations   = CacheReader2.getLeaseTerminations(cacheDir);
            _lastClosedDate = CacheReader2.getLastClosedDay(cacheDir);

            return CacheReader2.getLeases(cacheDir).Values
                               .Where    (_ => _.Rent.Interval == ReportModels.BillInterval.Daily)
                               .Select   (_ => _ as object).ToList();
        }

        public override int GetByfId(object byf)
            => (int)Cast(byf).Id.Value;


        private ReportModels.Lease Cast(object rec) 
            => Cast<ReportModels.Lease>(rec);


        public override void ReplaceAll(IEnumerable<IDocumentDTO> records, MarketStateDB mkt)
        {
            var actives = records.Where (_ => !(_ is InactiveLeaseDTO))
                                 .Select(_ => _ as LeaseDTO);
            
            var inactvs = records.Where (_ => _ is InactiveLeaseDTO)
                                 .Select(_ => _ as InactiveLeaseDTO);
            
            mkt.ActiveLeases  .DropAndInsert(actives, true, false);
            mkt.InactiveLeases.DropAndInsert(inactvs, true, false);

            //var tupl = records.SegregateByTermination(_lastClosedDate);
            //mkt.ActiveLeases  .DropAndInsert(tupl.Actives  , true, false);
            //mkt.InactiveLeases.DropAndInsert(tupl.Inactives, true, false);
        }
    }
}
