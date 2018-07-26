using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.PrintTools;
using RentLog.ChequeVouchers.DcdrTab.PassbookRows;
using System;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.DcdrTab
{
    public class DcdrPrintTrigger
    {
        public DcdrPrintTrigger(MainWindowVM main)
        {
            main.PrintClicked += (a, b) =>
            {
                if (main.SelectedTab == MainTabs.DcdrReport)
                    AskToPrintTargetDatagrid(main);
            };
        }


        public DataGrid TargetDatagrid { get; set; }


        private void AskToPrintTargetDatagrid(MainWindowVM main)
        {
            if (TargetDatagrid == null)
                throw Null.Ref(nameof(TargetDatagrid));

            var vm = main.DcdrReport.PassbookRows;

            TargetDatagrid.AskToPrint(GetHeaderLeftText(vm),
                                      GetHeaderCenterText(vm),
                                      GetHeaderRightText(vm));
        }

        private string GetHeaderLeftText(PassbookRowsVM vm)
            => vm.AppArgs.MarketState.BranchName;

        private string GetHeaderCenterText(PassbookRowsVM vm)
            => vm.AppArgs.CurrentBankAcct.Name;

        private string GetHeaderRightText(PassbookRowsVM vm)
            => $"running balance: {vm.LastRow.RunningBalance:N2}" + L.f 
             + $"as of: {vm.LastRow.TransactionDate.ToString("MMMM d, yyyy")}";
    }
}
