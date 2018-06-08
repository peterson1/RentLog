using RentLog.ChequeVouchers.VoucherReqsTab.FundRequests;
using RentLog.ChequeVouchers.VoucherReqsTab.IssuedCheques;
using RentLog.ChequeVouchers.VoucherReqsTab.PreparedCheques;
using RentLog.DomainLib11.DataSources;

namespace RentLog.ChequeVouchers.VoucherReqsTab
{
    public class VoucherReqsTabVM
    {
        public VoucherReqsTabVM(ITenantDBsDir dir)
        {
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
