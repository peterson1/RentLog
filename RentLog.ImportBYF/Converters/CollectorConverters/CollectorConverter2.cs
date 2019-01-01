using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.CollectorConverters
{
    public class CollectorConverter2 : ConverterRowBase<CollectorDTO>
    {
        public override string Label          => "Collectors";
        public override string ViewsDisplayID => "collectors_list?display_id=page_2";

        public CollectorConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override CollectorDTO CastToRNT(dynamic byf) => new CollectorDTO
        {
            Id       = As.ID(byf.nid),
            Name     = As.Text(byf.name),
            IsActive = As.Bool_(byf.isactive) ?? false,
            Remarks  = As.Text(byf.contactinfo)
        };


        public override List<CollectorDTO> GetRntRecords(ITenantDBsDir dir)
            => dir.MarketState.Collectors.GetAll();


        public override void ReplaceAll(IEnumerable<CollectorDTO> newRecords, MarketStateDbBase mkt)
            => mkt.Collectors.DropAndInsert(newRecords, true, false);
    }
}
