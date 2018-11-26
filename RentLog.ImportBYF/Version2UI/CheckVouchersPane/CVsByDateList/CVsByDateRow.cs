using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.RntCommands;
using RentLog.ImportBYF.RntQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList
{
    [AddINotifyPropertyChangedInterface]
    public class CVsByDateRow
    {
        public CVsByDateRow(DateTime date, MainWindowVM2 mainWindowVM2)
        {
            Date         = date;
            MainWindow   = mainWindowVM2;
            RefreshCmd   = R2Command.Async(FillBothCells, _ => !IsBusy);
            UpdateRntCmd = R2Command.Async(UpdateRnt, _ => CanUpdateRnt(), "Import");
        }


        public IR2Command      RefreshCmd     { get; }
        public IR2Command      UpdateRntCmd   { get; }
                                              
        public DateTime        Date           { get; }
        public MainWindowVM2   MainWindow     { get; }
        public bool            IsBusy         { get; private set; }
        public CVsByDateCell   ByfCell        { get; private set; }
        public CVsByDateCell   RntCell        { get; private set; }
        public bool?           IsValidImport  { get; private set; }
        public string          Status         { get; set; }
        public string          WhyInvalid     { get; set; }
        public string          Errors         { get; set; }


        private async Task FillBothCells()
        {
            StartBeingBusy("Querying both sources ...");
            try
            {
                await FillByfCell ();
                await FillRntCell ();
                CompareByfVsRnt   ();
            }
            catch (Exception ex)
            {
                IsValidImport = false;
                Errors        = ex.Info(true, true);
            }
            StopBeingBusy();
        }


        private async Task FillByfCell()
        {
            ByfCell = await MainWindow.QueryCheckVouchers(Date);
        }


        private async Task FillRntCell()
        {
            await Task.Run(()
                => RntCell = MainWindow.AppArgs.GetCheckVouchers(Date));
        }


        public bool CanUpdateRnt()
        {
            if (IsBusy) return false;
            if (ByfCell == null) return false;
            if (!ByfCell.IsQueried) return false;
            if (!MainWindow.ByfCache.IsFilled) return false;
            if (IsValidImport == true) return false;
            return true;
        }


        private async Task UpdateRnt()
        {
            StartBeingBusy("Updating local Rnt DB ...");
            try
            {
                await Task.Run(() => this.UpdateCheckVouchers());
                await FillRntCell();
                CompareByfVsRnt();
            }
            catch (Exception ex)
            {
                IsValidImport = false;
                Errors = ex.Info(true, true);
            }
            StopBeingBusy();
        }


        private void CompareByfVsRnt()
        {
            if (RntCell == null)
            {
                IsValidImport = false;
                WhyInvalid = "Rnt cell is NULL";
                return;
            }
            IsValidImport = RntCell.Matches(ByfCell, out string whyNot);
            WhyInvalid = whyNot;
        }


        public void StartBeingBusy (string jobDescription)
        {
            IsBusy = true;
            Status = jobDescription;
        }
        public void StopBeingBusy  (string message = null)
        {
            IsBusy = false;
            Status = message;
        }
    }
}
