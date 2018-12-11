using CommonTools.Lib11.DTOs;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.CollectorConverters
{
    public class CollectorConverter1 : ComparisonsListBase
    {
        public CollectorConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = Cast(byfRecord);
            return new CollectorDTO
            {
                Id       = (int)byf.Id.Value,
                Name     = byf.Name,
                IsActive = byf.IsActive,
            };
        }


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir dir)
            => dir.MarketState.Collectors.GetAll()
                .Select(_ => _ as IDocumentDTO).ToList();


        public override List<object> GetListFromBYF(string cacheDir)
            => CacheReader2.getCollectors(cacheDir).Values
                .Select(_ => _ as object).ToList();


        public override int GetByfId(object byfRecord)
            => (int)Cast(byfRecord).Id.Value;


        private ReportModels.Collector Cast(object rec)
            => Cast<ReportModels.Collector>(rec);


        public override void ReplaceAll(IEnumerable<IDocumentDTO> docs, MarketStateDbBase mkt) 
            => mkt.Collectors.DropAndInsert(docs
                .Select(_ => _ as CollectorDTO), true, false);
    }
}
