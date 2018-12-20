using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
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

            foreach (var byf in byfAdjs)
            {
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

                var lse = windowVM.Lease;
                var bals = windowVM.AppArgs.Balances.GetRepo(lse);

                windowVM.StartBeingBusy("Recomputing ...");
                bals.RecomputeFrom(this.First().Date);
            });
            windowVM.StopBeingBusy();
        }
    }
}
