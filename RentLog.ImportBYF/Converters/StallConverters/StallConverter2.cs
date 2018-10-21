using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.StallConverters
{
    public class StallConverter2 : ConverterRowBase<StallDTO>
    {
        public override string Label          => "Stalls";
        public override string ViewsDisplayID => "stalls_list?display_id=page_2";

        public StallConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override StallDTO CastToRNT(dynamic byf)
        {
            var nid    = AsID(byf.nid);
            var sec    = FindSection(AsID(byf.sectionnid));
            var rent   = FindDefaultRent(nid);
            var rights = FindDefaultRights(nid);
            return new StallDTO
            {
                Id            = nid,
                Name          = AsText(byf.label),
                Remarks       = AsText(byf.remarks),
                IsOperational = AsBool(byf.operational),
                Section       = sec,
                DefaultRent   = rent,
                DefaultRights = rights,
            };
        }


        private SectionDTO FindSection(dynamic dynamic)
        {
            throw new NotImplementedException();
        }


        private RentParams FindDefaultRent(dynamic nid)
        {
            throw new NotImplementedException();
        }


        private RightsParams FindDefaultRights(dynamic nid)
        {
            throw new NotImplementedException();
        }


        public override List<StallDTO> GetRntRecords(ITenantDBsDir dir)
            => dir.MarketState.Stalls.GetAll();


        public override void ReplaceAll(IEnumerable<StallDTO> newRecords, MarketStateDB mkt)
            => mkt.Stalls.DropAndInsert(newRecords, true, false);
    }
}
