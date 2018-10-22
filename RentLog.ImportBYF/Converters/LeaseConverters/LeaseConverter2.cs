using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public class LeaseConverter2 : ConverterRowBase<LeaseDTO>
    {
        public override string Label          => "Leases";
        public override string ViewsDisplayID => "leases_list?display_id=page_2";

        public LeaseConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override LeaseDTO CastToRNT(dynamic byf)
        {
            var period = GetContractDates((string)byf.contractstartend);
            return new LeaseDTO
            {
                Id                   = AsID(byf.nid),
                ContractStart        = period.Start,
                ContractEnd          = period.End,
                ProductToSell        = AsText(byf.producttosell),
                ApplicationSubmitted = AsDate(byf.formsubmitteddate),
                Remarks              = AsText(byf.remarks),
                //Rent                 = 
            };
        }


        private (DateTime Start, DateTime End) GetContractDates(string periodText)
        {
            var split = periodText.SplitTrim("to");
            return (DateTime.Parse(split[0]).ToLocalTime(),
                    DateTime.Parse(split[1]).ToLocalTime());
        }


        public override List<LeaseDTO> GetRntRecords(ITenantDBsDir dir) 
            => dir.MarketState.GetAllLeases();


        public override void ReplaceAll(IEnumerable<LeaseDTO> newRecords, MarketStateDB mkt)
        {
            var actives = newRecords.Where(_ => !(_ is InactiveLeaseDTO));
            var inactvs = newRecords.Where(_ =>   _ is InactiveLeaseDTO)
                                    .Select(_ => _ as InactiveLeaseDTO);

            mkt.ActiveLeases  .DropAndInsert(actives, true, false);
            mkt.InactiveLeases.DropAndInsert(inactvs, true, false);
        }
    }
}
