using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public class AllActiveLeasesVM : FilteredListVMBase
    {
        //private LeaseCRUD1VM _crud;


        public AllActiveLeasesVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
        {
            var all = mkt.ActiveLeases.GetAll();
            var filtered = secId == 0 ? all
                         : all.Where(_ => _.Stall.Section.Id == secId);
            return filtered.OrderByDescending(_ => _.Id).ToList();
        }


        protected override IR2Command CreateEncodeNewDraftCmd(ITenantDBsDir dir)
        {
            //_crud = new LeaseCRUD1VM(dir);
            return LeaseCRUD1VM.GetEncodeNewDraftCmd(dir, () => this.ClickRefresh());
        }
    }
}
