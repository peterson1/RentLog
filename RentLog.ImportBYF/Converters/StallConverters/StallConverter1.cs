using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.StallConverters
{
    public class StallConverter1 : ComparisonsListBase
    {
        private List<ReportModels.Lease> _byfLeases;


        public StallConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }


        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = Cast(byfRecord);
            return new StallDTO
            {
                Id            = (int)byf.Id.Value,
                Name          = byf.Label.Value.Trim(),
                IsOperational = byf.IsOperational,
                Section       = FindSection(byf),
                DefaultRent   = FindDefaultRent(byf),
                DefaultRights = FindDefaultRights(byf),
            };
        }


        private SectionDTO FindSection(ReportModels.Stall byf)
        {
            var secId = (int)byf.Section.Id.Value;
            return AppArgs.MarketState.Sections.Find(secId, true);
        }


        private RentParams FindDefaultRent(ReportModels.Stall byf) 
            => _byfLeases.FindLatestOccupancy(byf).Rent;


        private RightsParams FindDefaultRights(ReportModels.Stall byf)
            => _byfLeases.FindLatestOccupancy(byf).Rights;


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir dir)
            => dir.MarketState.Stalls.GetAll()
                .Select(_ => _ as IDocumentDTO).ToList();


        public override List<object> GetListFromBYF(string cacheDir)
        {
            _byfLeases = CacheReader2.getLeases(cacheDir)
                                     .Values.ToList();

            return CacheReader2.getStalls(cacheDir).Values
                               .Select(_ => _ as object).ToList();
        }


        public override int GetByfId(object byfRecord)
            => (int)Cast(byfRecord).Id.Value;


        private ReportModels.Stall Cast(object rec)
            => Cast<ReportModels.Stall>(rec);


        public override void ReplaceAll(IEnumerable<IDocumentDTO> docs, MarketStateDB mkt)
            => mkt.Stalls.DropAndInsert(docs
                .Select(_ => _ as StallDTO), true, false);
    }
}
