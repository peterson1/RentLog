using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList
{
    [AddINotifyPropertyChangedInterface]
    public class MemosByDateListVM : UIList<MemosByDateRow>
    {
        public MemosByDateListVM(MainWindowVM2 mainWindowVM2)
        {
            MainWindow = mainWindowVM2;
            MainWindow.ByfServer.GotMinMaxDates += ByfServer_GotMinMaxDates;
        }


        public MainWindowVM2  MainWindow  { get; }


        private void ByfServer_GotMinMaxDates(object sender, (DateTime Min, DateTime Max) dates)
        {
            var list = dates.Min.EachDayUpTo(dates.Max.AddDays(4))
                            .OrderByDescending(_ => _)
                            .Select(_ => new MemosByDateRow(_, MainWindow));
            UIThread.Run(() => SetItems(list));
        }


        public Task RefreshAll() => ProcessEachRow(this);

        public Task RefreshAllReversed() => ProcessEachRow
            (this.Where   (_ => _.Date.Year >= 2015)
                 .OrderBy (_ => _.Date));


        private async Task ProcessEachRow(IEnumerable<MemosByDateRow> rows)
        {
            foreach (var row in rows)
            {
                SetStatus($"Refreshing [{row.Date:d-MMM-yyyy}] ...");
                if (!ShouldKeepRunning) return;

                await row.RefreshCmd.RunAsync();
                if (!ShouldKeepRunning) return;

                if (row.CanUpdateRnt())
                    await row.UpdateRntCmd.RunAsync();

                await Task.Delay(100);
            }
            SetStatus("All rows refreshed.");
        }


        private void SetStatus(string message) 
            => MainWindow.BankMemos.Status = message;


        private bool ShouldKeepRunning => MainWindow.BankMemos.IsRunning;
    }
}
