﻿using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.PrintTools;
using PropertyChanged;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45;
using RentLog.DomainLib45.WithOverduesReport;

namespace RentLog.LeasesCrud.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private MainWindowVM _main;
        private AppArguments _args;


        public MainToolbarVM(MainWindowVM mainWindowVM)
        {
            _main                 = mainWindowVM;
            _args                 = _main.AppArgs;
            WithOverduesReportCmd = WithOverduesReport.CreateLauncherCmd(_args);
            PrintCurrentListCmd   = R2Command.Relay(PrintCurrentList, null, "Print Current List");
            ExportListToExcelCmd  = R2Command.Relay(ExportListToExcel, null, "Export List to Excel");
        }


        public IR2Command   WithOverduesReportCmd  { get; }
        public IR2Command   PrintCurrentListCmd    { get; }
        public IR2Command   ExportListToExcelCmd   { get; }
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
    }
}