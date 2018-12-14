using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.AdHocJobs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Threading.Tasks;

namespace RentLog.FilteredLeases.FilteredLists.AllActiveLeases
{
    public class AdhocLeaseJobs
    {
        public static IR2Command CreateLeaseJobCmd(int taskNumber, FilteredListVMBase listVM)
        {
            return R2Command.Relay(() =>
            {
                Action<LeaseDTO, ITenantDBsDir> job = null;
                string desc = null;
                switch (taskNumber)
                {
                    case 1: job = SetToMonthly(out desc); break;
                    case 2: if (!PopUpInput.TryGetDate("Look for Balances starting from", out DateTime date)) break;
                            job = ForSpecificLease.FindMemosFrom(date, out desc); break;
                    default: Alert.Show($"Unrecognized task number: [{taskNumber}]"); break;
                }

                Alert.Confirm($"Run “{desc}”?", async () =>
                {
                    listVM.Main.StartBeingBusy($"Running “{desc}” ...");

                    var dir = listVM.AppArgs;
                    var lse = listVM.Rows.CurrentItem?.DTO;
                    await Task.Run(() => job.Invoke(lse, dir));

                    listVM.Main.StopBeingBusy();
                    listVM.DoAfterSave.Invoke();
                });
            }, 
            CanRun(listVM), $"Run Ad Hoc command {taskNumber}");
        }


        private static Predicate<object> CanRun(FilteredListVMBase listVM) => _ 
            => (listVM.Rows.CurrentItem?.DTO != null) 
                && listVM.AppArgs.CanRunAdHocTask(false);


        private static Action<LeaseDTO, ITenantDBsDir> SetToMonthly (out string taskDescription)
        {
            taskDescription = "Set Interval to ‹Monthly›";
            return (lse, dir) =>
            {
                lse.Rent.Interval     = BillInterval.Monthly;
                lse.Rent.PenaltyRule  = RentPenalty.MonthlySurcharge;
                lse.Rent.PenaltyRate1 = 0.03M;
                lse.Rent.PenaltyRate2 = 5;
                dir.MarketState.ActiveLeases.Update(lse);
            };
        }
    }
}
