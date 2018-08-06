using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests;
using RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques;
using RentLog.ChequeVouchers.VoucherReqsTab.PreparedCheques;
using RentLog.ChequeVouchers.VoucherReqsTab.VouchersListPrint;
using RentLog.DomainLib11.DataSources;

namespace RentLog.ChequeVouchers.VoucherReqsTab
{
    public class VoucherReqsTabVM
    {
        private VouchersListPrintTrigger _printr;


        public VoucherReqsTabVM(MainWindowVM mainWindowVM)
        {
            var dir         = mainWindowVM.AppArgs;
            _printr         = new VouchersListPrintTrigger(mainWindowVM);
            FundRequests    = new FundReqsListVM(dir);
            PreparedCheques = new PreparedChequesListVM(dir);
            IssuedCheques   = new IssuedChequesListVM(dir);
        }


        public FundReqsListVM         FundRequests     { get; }
        public PreparedChequesListVM  PreparedCheques  { get; }
        public IssuedChequesListVM    IssuedCheques    { get; }


        public void ReloadAll()
        {
            FundRequests   .ReloadFromDB();
            PreparedCheques.ReloadFromDB();
            IssuedCheques  .ReloadFromDB();
        }
    }
}
