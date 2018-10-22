using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.StallConverters
{
    public class StallConverter2 : ConverterRowBase<StallDTO>
    {
        public override string Label          => "Stalls";
        public override string ViewsDisplayID => "stalls_list?display_id=page_2";

        private List<LeaseDTO> _lses;


        public StallConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override StallDTO CastToRNT(dynamic byf)
        {
            var nid    = As.ID(byf.nid);
            var sec    = FindSection(As.ID(byf.sectionnid));
            var rent   = FindDefaultRent(nid);
            var rights = FindDefaultRights(nid);
            return new StallDTO
            {
                Id            = nid,
                Name          = As.Text(byf.label),
                Remarks       = As.Text(byf.remarks),
                IsOperational = As.Bool(byf.operational),
                Section       = sec,
                DefaultRent   = rent,
                DefaultRights = rights,
            };
        }


        private SectionDTO FindSection(int sectionId)
            => Main.AppArgs.MarketState.Sections.Find(sectionId, true);


        private RentParams FindDefaultRent(int stallId) 
            => GetLatestOccupancy(stallId)?.Rent;


        private RightsParams FindDefaultRights(int stallId)
            => GetLatestOccupancy(stallId)?.Rights;


        private LeaseDTO GetLatestOccupancy(int stallId)
            => _lses?.Where  (_ => _.Stall.Id == stallId)
                    ?.OrderBy(_ => _.Id)
                    ?.LastOrDefault();


        public override List<StallDTO> GetRntRecords(ITenantDBsDir dir)
        {
            _lses = dir.MarketState.GetAllLeases();
            return dir.MarketState.Stalls.GetAll();
        }

        public override void ReplaceAll(IEnumerable<StallDTO> newRecords, MarketStateDB mkt)
            => mkt.Stalls.DropAndInsert(newRecords, true, false);
    }
}
