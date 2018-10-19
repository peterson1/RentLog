using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.RntQueries;
using System;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.DailyTransactions
{
    [AddINotifyPropertyChangedInterface]
    public class DailyTransactionRow
    {
        public DailyTransactionRow(DateTime date, MainWindowVM mainWindowVM)
        {
            Date         = date;
            MainWindow   = mainWindowVM;
            RefreshCmd   = R2Command.Async(FillBothCells, _ => !IsBusy);
            QueryByfCmd  = R2Command.Async(FillByfCell, _ => !IsBusy, "Query BYF");
            UpdateRntCmd = R2Command.Async(this.UpdateRnt, _ => this.CanUpdateRnt(), "Update RNT");
        }


        public IR2Command  RefreshCmd    { get; }
        public IR2Command  QueryByfCmd   { get; }
        public IR2Command  UpdateRntCmd  { get; }

        public DateTime              Date          { get; }
        public MainWindowVM          MainWindow    { get; }
        public bool                  IsBusy        { get; private set; }
        public DailyTransactionCell  ByfCell       { get; private set; }
        public DailyTransactionCell  RntCell       { get; private set; }
        public bool                  IsValidImport { get; private set; }
        public string                Remarks       { get; private set; }


        private async Task FillBothCells()
        {
            StartBeingBusy("Querying both sources ...");
            string err;
            try
            {
                await FillByfCell();
                RntCell       = this.QueryRNT();
                IsValidImport = Validate(out err);
            }
            catch (Exception ex)
            {
                err = ex.Info(true, true);
            }
            StopBeingBusy(IsValidImport ? null : err);
        }


        private async Task FillByfCell()
        {
            StartBeingBusy("Querying BYF server ...");
            ByfCell = await this.CheckCache(MainWindow.CacheDir);
            StopBeingBusy();
        }


        public void StartBeingBusy(string jobDescription)
        {
            IsBusy = true;
            Remarks = jobDescription;
        }


        public void StopBeingBusy(string message = null)
        {
            IsBusy = false;
            Remarks = message;
        }


        private bool Validate(out string whyNot)
        {
            if (!ByfCell.IsBalanced)
            {
                whyNot = $"Imbalanced BYF -- [Colxns: {ByfCell.TotalCollections}]  -vs-  [Deps: {ByfCell.TotalDeposits}]  -diff= {ByfCell.TotalDiff}";
                return false;
            }
            if (!RntCell.IsBalanced)
            {
                whyNot = $"Imbalanced RNT -- [Colxns: {RntCell.TotalCollections}]  -vs-  [Deps: {RntCell.TotalDeposits}]  -diff= {RntCell.TotalDiff}";
                return false;
            }
            if (ByfCell.TotalDeposits != RntCell.TotalDeposits)
            {
                whyNot = $"[Byf: {ByfCell.TotalDeposits}]  -vs-  [Rnt: {RntCell.TotalDeposits}]";
                return false;
            }
            whyNot = "";
            return true;
        }
    }
}
