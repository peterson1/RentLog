using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Converters.BalanceAdjConverters;
using RentLog.ImportBYF.Version2UI;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    class VerifyLeaseMemosFix
    {
        internal static void Run(MainWindowVM2 main)
        {
            var mkt = main.AppArgs.MarketState;
            ProcessLeases(main, mkt.ActiveLeases);
            ProcessLeases(main, mkt.InactiveLeases);
            main.StopBeingBusy();
        }


        private static void ProcessLeases<T>(MainWindowVM2 main, ISimpleRepo<T> repo)
            where T : LeaseDTO
        {
            foreach (var lse in repo.GetAll())
            {
                main.StartBeingBusy($"Verifying memos for [{lse.Id}] “{lse}”...");

                var win = new LeaseBalAdjustmentsVM(lse, main);
                win.Show<LeaseBalAjsWindow>(false, true);

                //foreach (BalanceAdjustmentDTO byf in byfAdjs)
                //    VerifyLeaseMemo(byf, conv, main.AppArgs);
            }
        }


        //private static void VerifyLeaseMemo(BalanceAdjustmentDTO byf, BalanceAdjConverter1 conv, ITenantDBsDir dir)
        //{
        //    var date = conv._adjDates[byf.Id];
        //    var repo = dir.Collections.For(date).BalanceAdjs;
        //    var rnt  = repo.Find(byf.Id, false);

        //    if (rnt == null)
        //    {
        //        Alert.ShowModal($"Not imported for Lease [{byf.LeaseId}]",
        //                        $"BYF balance_adj nid: [{byf.Id}]");
        //    }

        //}
    }
}
