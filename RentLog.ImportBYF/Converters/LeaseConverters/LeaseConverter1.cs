using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public class LeaseConverter1 : ComparisonsListBase
    {
        public LeaseConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = Cast(byfRecord);
            return new LeaseDTO
            {
                Id            = (int)byf.Id.Value,
                ContractStart = byf.ContractStart,
                ContractEnd   = byf.ContractEnd,
                ProductToSell = byf.ProductToSell
            };
        }


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir appArgs)
        {
            var mkt     = appArgs.MarketState;
            var actives = mkt.ActiveLeases.GetAll()
                             .Select(_ => _ as IDocumentDTO);
            var inactvs = mkt.InactiveLeases.GetAll()
                             .Select(_ => _ as IDocumentDTO);
            return actives.Concat(inactvs).ToList();
        }


        public override List<object> GetListFromBYF(string cacheDir)
            => CacheReader2.getLeases(cacheDir).Values
                .Select(_ => _ as object).ToList();


        public override int GetByfId(object byf)
            => (int)Cast(byf).Id.Value;


        private ReportModels.Lease Cast(object rec) 
            => Cast<ReportModels.Lease>(rec);


        public override void ReplaceAll(IEnumerable<IDocumentDTO> records, MarketStateDB mkt)
        {
            throw new System.NotImplementedException();
        }
    }
}
