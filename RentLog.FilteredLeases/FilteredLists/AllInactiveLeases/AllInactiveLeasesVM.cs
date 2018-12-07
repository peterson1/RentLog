using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.StateTransitions;
using RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.FilteredLeases.FilteredLists.AllInactiveLeases
{
    public class AllInactiveLeasesVM : FilteredListVMBase
    {
        public AllInactiveLeasesVM(MainWindowVM main, ITenantDBsDir dir) : base(main, dir)
        {
            AddStallToTenantCmd = LeaseCRUD1VM.GetAddStallToTenantCmd(this, DoOnSave);
            UndoTerminationCmd  = R2Command.Relay(UndoTermination, 
                              _ => AppArgs.CanUndoLeaseTermination(false), 
                                    "Undo Lease Termination");
        }


        public IR2Command  AddStallToTenantCmd { get; }
        public IR2Command  UndoTerminationCmd  { get; }


        private void UndoTermination()
        {
            if (!TryGetPickedItem(out LeaseDTO lse)) return;
            var inactv = lse as InactiveLeaseDTO;

            Alert.Confirm($"Undo contract termination for “{lse}”?", () =>
            {
                AppArgs.MarketState.UndoLeaseTermination(inactv);
                Alert.Show($"“{lse}” is now Active.");
                ReloadFromDB();
            });
        }


        protected override List<LeaseDTO> GetLeases(MarketStateDB mkt, int secId)
        {
            var all = mkt.InactiveLeases.GetAll();
            var filtered = secId == 0 ? all
                         : all.Where(_ => _.Stall.Section.Id == secId);
            return filtered.OrderByDescending(_ => _.Id)
                           .Select (_ => _ as LeaseDTO)
                           .ToList ();
        }


        private Action DoOnSave => () => this.ClickRefresh();
    }
}
