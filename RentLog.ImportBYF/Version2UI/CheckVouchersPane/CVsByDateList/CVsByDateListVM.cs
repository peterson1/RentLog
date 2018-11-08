﻿using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList
{
    [AddINotifyPropertyChangedInterface]
    public class CVsByDateListVM : UIList<CVsByDateRow>
    {
        public CVsByDateListVM(MainWindowVM2 mainWindowVM2)
        {
            MainWindow = mainWindowVM2;
            MainWindow.ByfServer.GotMinMaxDates += ByfServer_GotMinMaxDates;
        }


        public MainWindowVM2  MainWindow  { get; }


        private void ByfServer_GotMinMaxDates(object sender, (DateTime Min, DateTime Max) dates)
        {
            var list = dates.Min.EachDayUpTo(dates.Max)
                            .OrderByDescending(_ => _)
                            .Select(_ => new CVsByDateRow(_, MainWindow));
            UIThread.Run(() => SetItems(list));
        }


        public async Task RefreshAll()
        {
            foreach (var row in this)
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
            => MainWindow.CheckVouchers.Status = message;


        private bool ShouldKeepRunning => MainWindow.CheckVouchers.IsRunning;
    }
}