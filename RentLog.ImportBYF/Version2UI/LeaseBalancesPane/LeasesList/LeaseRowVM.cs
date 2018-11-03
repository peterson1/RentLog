using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
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
            RefreshCmd   = R2Command.Async(LoadRntCell, _ => !IsBusy);
            UpdateRntCmd = R2Command.Async(UpdateRntDB, _ => !IsBusy, "Update RNT");
        }


        public LeaseDTO    Lease         { get; }
        public IR2Command  RefreshCmd    { get; }
        public IR2Command  UpdateRntCmd  { get; }

        public MainWindowVM2    MainWindow     { get; }
        public bool             IsBusy         { get; private set; }
        public BillAmounts      RntCell        { get; private set; }
        public bool?            IsValidImport  { get; private set; }
        public string           Remarks        { get; set; }
        public string           Errors         { get; set; }

        public bool IsActive => !(Lease is InactiveLeaseDTO);
        public string CompositeLabel => IsActive ? $"{Lease}"
                                        : $"[Inactive]  {Lease}";


        private async Task LoadRntCell()
        {
            StartBeingBusy($"Reading RNT balances for “{Lease}” ...");
            await Task.Run(() =>
            {
                try
                {
                    RntCell = MainWindow.GetBalances(Lease);
                    IsValidImport = Validate(out string err);
                    Remarks = err;
                }
                catch (Exception ex)
                {
                    IsValidImport = false;
                    Errors = ex.Info(true, true);
                }
            });
            StopBeingBusy();
        }


        private async Task UpdateRntDB()
        {
            StartBeingBusy($"Rewriting balances for “{Lease}” ...");
            await Task.Run(async () =>
            {
                try
                {
                    await this.UpdateBalances();
                    StopBeingBusy();

                }
                catch (Exception ex)
                {
                    Errors = ex.Info(true, true);
                }
            });
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
            var val = RntCell?.For(billCode);
            if (val.HasValue)
            {
                whyNot = "";
                return true;
            }
            whyNot = $"No balance for ‹{billCode}›.";
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
