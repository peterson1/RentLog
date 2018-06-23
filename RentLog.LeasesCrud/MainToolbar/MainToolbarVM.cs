using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.PrintTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45.WithOverduesReport;
using System;
using System.Threading.Tasks;

namespace RentLog.LeasesCrud.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private MainWindowVM  _main;
        private ITenantDBsDir _args;


        public MainToolbarVM(MainWindowVM mainWindowVM)
        {
            _main                 = mainWindowVM;
            _args                 = _main.AppArgs;
            WithOverduesReportCmd = WithOverduesReport.CreateLauncherCmd(_args);
            PrintCurrentListCmd   = R2Command.Relay(PrintCurrentList, null, "Print Current List");
            ExportListToExcelCmd  = R2Command.Relay(ExportListToExcel, null, "Export List to Excel");
            RunAdHocTaskCmd       = R2Command.Async(RunAdHocTask, _ => _args.CanRunAdHocTask(false), "Run Ad Hoc Script");
        }


        public IR2Command   WithOverduesReportCmd  { get; }
        public IR2Command   PrintCurrentListCmd    { get; }
        public IR2Command   ExportListToExcelCmd   { get; }
        public IR2Command   RunAdHocTaskCmd         { get; }
        public BillAmounts  Overdues               { get; private set; }


        private void PrintCurrentList()
        {
            var labl = _main.CurrentIsActive ? "Acive Leases"
                                             : "Inacive Leases";
            _main.CurrentTable.AskToPrint
                (labl, null, _args.MarketState.BranchName);
        }


        private void ExportListToExcel() 
            => _main.CurrentTable.ExportToExcel();


        public void UpdateAll() 
            => Overdues = _args.Balances.TotalOverdues();


        private async Task RunAdHocTask()
        {
            _main.StartBeingBusy("Running Ad Hoc task ...");
            await Task.Run(() =>
            {
                //AllLeasesNoRoundOff.Run(_args);
                //NonExpiringLeasesForSome.Run(_args);
                throw new InvalidCastException("blah!");
            });
            _main.StopBeingBusy();
            _main.ClickRefresh();
        }
    }
}
