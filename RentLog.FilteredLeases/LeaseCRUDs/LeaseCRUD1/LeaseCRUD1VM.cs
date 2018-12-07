using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45;
using RentLog.LeasesCrud.LeaseCRUD;
using System;

namespace RentLog.FilteredLeases.LeaseCRUDs.LeaseCRUD1
{
    public class LeaseCRUD1VM
    {
        private ITenantDBsDir _dir;
        private Action       _doWhenDone;


        public LeaseCRUD1VM(ITenantDBsDir tenantDBsDir, Action doWhenDone)
        {
            _dir        = tenantDBsDir;
            _doWhenDone = doWhenDone;
        }


        public static IR2Command GetEncodeNewDraftCmd(ITenantDBsDir dir, Action doWhenDone)
        {
            if (!dir.CanAddLease(false)) return null;
            var thisVM = new LeaseCRUD1VM(dir, doWhenDone);
            return R2Command.Relay(thisVM.LaunchOldForInserting);
        }


        private void LaunchOldForInserting()
        {
            var repo = _dir.MarketState.ActiveLeases;
            var oldCrud = new LeaseCrudVM(repo, _dir as AppArguments);
            oldCrud.SaveCompleted += (s, e) => _doWhenDone.Invoke();
            oldCrud.EncodeNewDraftCmd.ExecuteIfItCan();
        }
    }
}
