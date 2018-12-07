using CommonTools.Lib11.InputCommands;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public class AllActiveLeasesVM : FilteredListVMBase
    {
        public AllActiveLeasesVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
            AddStallToTenantCmd   = LeaseCRUD1VM.GetAddStallToTenantCmd  (this, DoOnSave);
            EditThisLeaseCmd      = LeaseCRUD1VM.GetEditThisLeaseCmd     (this, DoOnSave);
            EditTenantInfoCmd     = LeaseCRUD1VM.GetEditTenantInfoCmd    (this, DoOnSave);
            TerminateThisLeaseCmd = LeaseCRUD1VM.GetTerminateThisLeaseCmd(this, DoOnSave);
        }

        public IR2Command   AddStallToTenantCmd    { get; }
        public IR2Command   EditThisLeaseCmd       { get; }
        public IR2Command   EditTenantInfoCmd      { get; }
        public IR2Command   TerminateThisLeaseCmd  { get; }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
        {
            var all = mkt.ActiveLeases.GetAll();
            var filtered = secId == 0 ? all
                         : all.Where(_ => _.Stall.Section.Id == secId);
            return filtered.OrderByDescending(_ => _.Id).ToList();
        }


        protected override IR2Command CreateEncodeNewDraftCmd(ITenantDBsDir dir)
            => LeaseCRUD1VM.GetEncodeNewDraftCmd(this, DoOnSave);


        private Action DoOnSave => () => this.ClickRefresh();
    }
}
