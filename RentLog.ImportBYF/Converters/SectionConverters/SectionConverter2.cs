using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.SectionConverters
{
    public class SectionConverter2 : ConverterRowBase<SectionDTO>
    {
        public override string Label          => "Sections";
        public override string ViewsDisplayID => "sections_list?display_id=page_3";

        public SectionConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override SectionDTO CastToRNT(dynamic byf)
        {
            var nme = As.Text(byf.label);
            return new SectionDTO
            {
                Id            = As.ID(byf.nid),
                Name          = nme,
                IsActive      = true,
                StallTemplate = StallDTO.Named(nme + " {0:000}"),
            };
        }


        public override List<SectionDTO> GetRntRecords(ITenantDBsDir dir)
            => dir.MarketState.Sections.GetAll();


        public override void ReplaceAll(IEnumerable<SectionDTO> newRecords, MarketStateDB mkt)
            => mkt.Sections.DropAndInsert(newRecords, true, false);
    }
}
