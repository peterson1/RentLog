using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.SectionConverters
{
    public class SectionConverter1 : ComparisonsListBase
    {
        public SectionConverter1(MainWindowVM mainWindowVM) : base(mainWindowVM)
        {
        }

        public override IDocumentDTO CastByfToDTO(object byfRecord)
        {
            var byf = Cast(byfRecord);
            var nme = byf.Label.Value.Trim();
            return new SectionDTO
            {
                Id            = (int)byf.Id.Value,
                Name          = nme,
                StallTemplate = StallDTO.Named(nme + " {0:000}"),
                IsActive      = byf.IsOperational
            };
        }


        public override List<IDocumentDTO> GetListFromRNT(ITenantDBsDir dir)
            => dir.MarketState.Sections.GetAll()
                .Select(_ => _ as IDocumentDTO).ToList();


        public override List<object> GetListFromBYF(string cacheDir)
            => CacheReader2.getSections(cacheDir).Values
                .Select(_ => _ as object).ToList();


        public override void ReplaceAll(IEnumerable<IDocumentDTO> records, MarketStateDB mkt)
            => mkt.Sections.DropAndInsert(records
                .Select(_ => _ as SectionDTO), true, false);


        public override int GetByfId(object byfRecord)
            => (int)Cast(byfRecord).Id.Value;


        private ReportModels.Section Cast(object rec)
            => Cast<ReportModels.Section>(rec);
    }
}
