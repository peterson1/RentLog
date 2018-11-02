using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class LeasesListVM : UIList<LeaseRowVM>
    {
        public LeasesListVM(MainWindowVM2 mainWindowVM2)
        {
            MainWindow = mainWindowVM2;
            this.ItemOpened += (s, e) 
                => SoaViewer.Show(e.Lease, MainWindow.AppArgs);
        }


        public MainWindowVM2 MainWindow { get; }


        public void FillLeasesList()
        {
            var mkt     = MainWindow.AppArgs.MarketState;
            var actives = mkt.ActiveLeases.GetAll()
                             .OrderByDescending(_ => _.Id);
            var inactvs = mkt.InactiveLeases.GetAll()
                             .OrderByDescending(_ => _.Id);

            var list = actives.Concat(inactvs)
                        .Select(_ => new LeaseRowVM(_, MainWindow));
            //var both    = actives.Concat(inactvs).ToList();
            //var list    = new List<LeaseRowVM>();
            //
            //for (int i = 0; i < both.Count; i++)
            //    list.Add(new LeaseRowVM(i, both[i], MainWindow));

            UIThread.Run(() => SetItems(list));
        }


        public async Task RefreshAll()
        {
            var jobs = new List<Task>();

            for (int i = 0; i < this.Count; i++)
                jobs.Add(UpdateAsNeeded(this[i], i * 200));

            //foreach (var job in jobs)
            //    await job;
            await Task.WhenAll(jobs);
        }


        private async Task UpdateAsNeeded(LeaseRowVM row, int delayMS)
        {
            await Task.Delay(delayMS);
            await row.RefreshCmd.RunAsync();
            if (!MainWindow.LeaseBalances.IsRunning) return;

            if (!row.IsValidImport)
            {
                await row.UpdateRntCmd.RunAsync();
                await row.RefreshCmd.RunAsync();
            }

            MainWindow.LeaseBalances.ShowCompleted(row);
        }
    }
}
