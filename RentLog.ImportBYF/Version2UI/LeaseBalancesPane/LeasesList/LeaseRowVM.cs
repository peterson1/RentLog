using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Threading.Tasks;
using RentLog.ImportBYF.RntQueries;
using RentLog.ImportBYF.RntCommands;

namespace RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseRowVM
    {
        public LeaseRowVM(LeaseDTO leaseDTO, MainWindowVM2 mainWindowVM2)
        {
            Lease        = leaseDTO;
            MainWindow   = mainWindowVM2;
            RefreshCmd   = R2Command.Async(FillBothCells, _ => !IsBusy);
            QueryByfCmd  = R2Command.Async(FillByfCell, _ => !IsBusy, "Query BYF");
            UpdateRntCmd = R2Command.Async(_ => this.UpdateRNT(), _ => CanUpdateRnt(), "Update RNT");
        }


        public LeaseDTO    Lease         { get; }
        public IR2Command  RefreshCmd    { get; }
        public IR2Command  QueryByfCmd   { get; }
        public IR2Command  UpdateRntCmd  { get; }

        public MainWindowVM2    MainWindow     { get; }
        public bool             IsBusy         { get; private set; }
        public BillAmounts      ByfCell        { get; private set; }
        public BillAmounts      RntCell        { get; private set; }
        public bool             IsValidImport  { get; private set; }
        public string           Remarks        { get; set; }


        private async Task FillBothCells()
        {
            StartBeingBusy("Querying both sources ...");
            string err;
            try
            {
                await FillByfCell();
                RntCell       = MainWindow.AppArgs.GetBalances(Lease);
                IsValidImport = Validate(out err);
            }
            catch (Exception ex)
            {
                err = ex.Info(true, true);
            }
            StopBeingBusy(IsValidImport ? null : err);
        }


        private bool CanUpdateRnt()
        {
            if (IsBusy) return false;
            if (ByfCell == null) return false;
            if (!ByfCell.HasValue) return false;
            if (!MainWindow.ByfCache.IsFilled) return false;
            return true;
        }


        private async Task FillByfCell()
        {
            StartBeingBusy("Querying BYF server ...");
            ByfCell = await MainWindow.ByfServer.QueryLeaseBalances(Lease);
            StopBeingBusy();
        }


        private bool Validate(out string whyNot)
        {
            if (!Validate(BillCode.Rent, out whyNot)) return false;
            if (!Validate(BillCode.Rights, out whyNot)) return false;
            //if (!Validate(BillCode.Electric, out whyNot)) return false;
            //if (!Validate(BillCode.Water, out whyNot)) return false;
            whyNot = "";
            return true;
        }


        private bool Validate(BillCode billCode, out string whyNot)
        {
            var byf = ByfCell?.For(billCode);
            var rnt = RntCell?.For(billCode);
            if (byf == rnt)
            {
                whyNot = "";
                return true;
            }
            whyNot = $"‹{billCode}›  [BYF:{byf:N2}]  -vs-  [RNT:{rnt:N2}]";
            return false;
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
    }
}
