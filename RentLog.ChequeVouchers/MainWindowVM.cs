using RentLog.ChequeVouchers.DcdrTab;
using RentLog.ChequeVouchers.VoucherReqsTab;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers
{
    public class MainWindowVM : BrandedWindowBase
    {
        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            VoucherReqs = new VoucherReqsTabVM(tenantDBsDir);
            DcdrReport  = new DcdrTabVM();
            ClickRefresh();
        }


        public VoucherReqsTabVM  VoucherReqs    { get; }
        public DcdrTabVM         DcdrReport     { get; }
        public int               SelectedIndex  { get; set; }


        public override string SubAppName => "Cheque Vouchers  |  DCDR";


        protected override async Task OnRefreshClickedAsync()
        {
            StartBeingBusy($"Loading Cheque Vouchers ...");

            if (SelectedIndex == 0)
                await Task.Run(() => VoucherReqs.ReloadAll());
            else
                await Task.Run(() => DcdrReport.ReloadAll());

            StopBeingBusy();
        }
    }
}
