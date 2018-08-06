using CommonTools.Lib45.PrintTools.PrintPreviewer2;

namespace RentLog.ChequeVouchers.VoucherReqsTab.VouchersListPrint
{
    public class VouchersListPrintTrigger
    {
        public VouchersListPrintTrigger(MainWindowVM main)
        {
            main.PrintClicked += (a, b) =>
            {
                if (main.SelectedTab == MainTabs.CheckVouchers)
                    AskToPrintVouchersList(main);
            };
        }


        public string MyProperty { get; set; } = "not implemented";


        private void AskToPrintVouchersList(MainWindowVM main)
        {
            PrintPreviewer2.Preview(this)
                .On<VouchersListPrintLayout1>();
        }
    }
}
