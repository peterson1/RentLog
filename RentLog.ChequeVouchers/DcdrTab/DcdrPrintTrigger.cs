using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.PrintTools;
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
                    AskToPrintTargetDatagrid();
            };
        }


        public DataGrid TargetDatagrid { get; set; }


        private void AskToPrintTargetDatagrid()
        {
            if (TargetDatagrid == null)
                throw Null.Ref(nameof(TargetDatagrid));

            TargetDatagrid.AskToPrint(GetHeaderLeftText(),
                                      GetHeaderCenterText());
        }


        private string GetHeaderCenterText()
        {
            throw new NotImplementedException();
        }

        private string GetHeaderLeftText()
        {
            return "DCDR";
        }
    }
}
