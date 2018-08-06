using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.PrintTools.PrintPreviewer2;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ChequeVouchers.VoucherReqsTab.VouchersListPrint
{
    public class VouchersListPrintTrigger
    {
        public VouchersListPrintTrigger(MainWindowVM mainWindowVM)
        {
            Main = mainWindowVM;
            Main.PrintClicked += (a, b) =>
            {
                if (mainWindowVM.SelectedTab == MainTabs.CheckVouchers)
                {
                    Outstandings.SetItems(GetOutstandingChecks());
                    AskToPrintVouchersList(mainWindowVM);
                }
            };
        }


        public MainWindowVM             Main            { get; }
        public UIList<ChequeVoucherDTO> Outstandings    { get; } = new UIList<ChequeVoucherDTO>();
        public UIList<FundRequestDTO>   ForPreparations => Tab?.FundRequests.ItemsList;
        public MarketStateDB            MarketState     => Main?.AppArgs.MarketState;
        public VoucherReqsTabVM         Tab             => Main?.VoucherReqs;


        private IEnumerable<ChequeVoucherDTO> GetOutstandingChecks()
                    => Tab?.IssuedCheques?.ItemsList?
               .Concat(Tab?.PreparedCheques?.ItemsList);


        private void AskToPrintVouchersList(MainWindowVM main)
        {
            PrintPreviewer2.Preview(this)
                .On<VouchersListPrintLayout1>();
        }
    }
}
