using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Converters.LeaseConverters;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.StallConverters
{
    public class StallConverter1 : ComparisonsListBase
    {
        private List<ReportModels.Lease> _byfLeases;
        private LeaseConverter1          _lseConv;


        public StallConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
            _lseConv = new LeaseConverter1(mainWindowVM);
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
            var sec   = AppArgs.MarketState.Sections.Find(secId, false);

            if (sec == null)
                throw No.Match<SectionDTO>("Id", secId);

            return sec;
        }


        private RentParams FindDefaultRent(ReportModels.Stall byf) 
            => _byfLeases.FindLatestOccupancy(byf, _lseConv)?.Rent;


        private RightsParams FindDefaultRights(ReportModels.Stall byf)
            => _byfLeases.FindLatestOccupancy(byf, _lseConv)?.Rights;


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


        public override void ReplaceAll(IEnumerable<IDocumentDTO> docs, MarketStateDbBase mkt)
            => mkt.Stalls.DropAndInsert(docs
                .Select(_ => _ as StallDTO), true, false);
    }
}
