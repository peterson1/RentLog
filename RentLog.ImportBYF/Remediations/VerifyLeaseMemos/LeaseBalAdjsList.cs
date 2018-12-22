using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Converters.BalanceAdjConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalAdjsList : UIList<LeaseBalAdjRow>
    {
        public async Task Fill(LeaseBalAdjustmentsVM windowVM)
        {
            var lse      = windowVM.Lease;
            var main     = windowVM.Main;
            var url      = BalanceAdjConverter1.VIEWS_ID;
            var conv     = new BalanceAdjConverter1(lse, main);
            var dynamics = await conv.GetViewsList(url);
            var byfAdjs  = dynamics.Select(_ => conv.CastToRNT(_));
            var rows     = new List<LeaseBalAdjRow>();

            foreach (BalanceAdjustmentDTO byf in byfAdjs)
            {
                if (conv._memoTypes[byf.Id] == 1) continue; //cashier colxn?
                if (byf.Remarks.Contains("Daily Rights Surcharge Memo")) continue;
                var row = new LeaseBalAdjRow(byf, conv, main.AppArgs);
                rows.Add(row);
            }
            UIThread.Run(() => this.SetItems
                (rows.OrderBy(_ => _.Date)));
        }


        public async Task Import(LeaseBalAdjustmentsVM windowVM)
        {
            windowVM.StartBeingBusy("Importing ...");
            await Task.Run(() =>
            {
                foreach (var row in this)
                    row.UpsertDTO();

                var lse  = windowVM.Lease;
                var bals = windowVM.AppArgs.Balances.GetRepo(lse);
                var day1 = bals.Earliest().GetBillDate();

                windowVM.StartBeingBusy("Recomputing ...");
                bals.RecomputeFrom(day1);
            });
            windowVM.StopBeingBusy();
        }
    }
}
