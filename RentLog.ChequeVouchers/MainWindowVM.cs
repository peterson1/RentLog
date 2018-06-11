using PropertyChanged;
using RentLog.ChequeVouchers.DcdrTab;
using RentLog.ChequeVouchers.MainToolbar;
using RentLog.ChequeVouchers.VoucherReqsTab;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            VoucherReqs    = new VoucherReqsTabVM(tenantDBsDir);
            DcdrReport     = new DcdrTabVM();
            BankAcctPicker = new BankAccountPickerVM(this);
        }


        public BankAccountPickerVM  BankAcctPicker  { get; }
        public VoucherReqsTabVM     VoucherReqs     { get; }
        public DcdrTabVM            DcdrReport      { get; }
        public int                  SelectedIndex   { get; set; }


        public override string SubAppName => "Cheque Vouchers  |  DCDR";


        protected override async Task OnRefreshClickedAsync()
        {
            StartBeingBusy($"Loading Cheque Vouchers ...");

            if (SelectedIndex == 0)
                await Task.Run(() => VoucherReqs.ReloadAll());
            else
                await Task.Run(() => DcdrReport.ReloadAll());

            SetCaption($"for “{AppArgs.CurrentBankAcct}”");

            StopBeingBusy();
        }
    }
}
