using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public class AdhocLeaseJobs
    {
        public static IR2Command CreateLeaseJobCmd(int taskNumber, FilteredListVMBase listVM)
        {
            return R2Command.Relay(() =>
            {
                var dir = listVM.AppArgs;
                var lse = listVM.Rows.CurrentItem?.DTO;
                switch (taskNumber)
                {
                    case 1 : SetToMonthly(lse, dir); break;
                    default: Alert.Show($"Unrecognized task number: [{taskNumber}]"); break;
                }
                listVM.DoAfterSave.Invoke();
            }, 
            CanRun(listVM), $"Run Ad Hoc command {taskNumber}");
        }


        private static Predicate<object> CanRun(FilteredListVMBase listVM) => _ 
            => (listVM.Rows.CurrentItem?.DTO != null) 
                && listVM.AppArgs.CanRunAdHocTask(false);


        private static void SetToMonthly(LeaseDTO lse, ITenantDBsDir dir)
        {
            lse.Rent.Interval = BillInterval.Monthly;
            dir.MarketState.ActiveLeases.Update(lse);
        }
    }
}
