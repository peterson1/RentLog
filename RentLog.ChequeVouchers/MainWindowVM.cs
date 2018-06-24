using PropertyChanged;
using RentLog.ChequeVouchers.DcdrTab;
using RentLog.ChequeVouchers.JournalsTab;
using RentLog.ChequeVouchers.MainToolbar;
using RentLog.ChequeVouchers.VoucherReqsTab;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using System;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Cheque Vouchers  |  DCDR";


        public MainWindowVM(ITenantDBsDir tenantDBsDir, bool clickRefresh = true) : base(tenantDBsDir)
        {
            DateRange      = new DateRangePickerVM();
            BankAcctPicker = new BankAccountPickerVM(this, clickRefresh);
            VoucherReqs    = new VoucherReqsTabVM(tenantDBsDir);
            DcdrReport     = new DcdrTabVM(this);
            Journals       = new JournalsTabVM(this);
        }


        public BankAccountPickerVM  BankAcctPicker  { get; }
        public DateRangePickerVM    DateRange       { get; }
        public VoucherReqsTabVM     VoucherReqs     { get; }
        public DcdrTabVM            DcdrReport      { get; }
        public JournalsTabVM        Journals        { get; }
        public int                  SelectedIndex   { get; set; }


        public void OnSelectedIndexChanged()
        {
            DateRange.IsVisible = SelectedIndex > 0;
            ClickRefresh();
        }


        protected override async Task OnRefreshClickedAsync()
        {
            StartBeingBusy($"Loading Cheque Vouchers ...");

            //if (SelectedIndex == 0)
            //    await Task.Run(() => VoucherReqs.ReloadAll());
            //else
            //    await Task.Run(() => DcdrReport.PassbookRows.ReloadFromDB());

            await Task.Run(PickRefreshJob());

            SetCaption($"for “{AppArgs.CurrentBankAcct}”");
            StopBeingBusy();
        }


        private Action PickRefreshJob() { switch (SelectedIndex)
        {
            case 0:  return () => VoucherReqs.ReloadAll();
            case 1:  return () => DcdrReport.PassbookRows.ReloadFromDB();
            default: return () => Journals.JournalRows.ReloadFromDB();
        }}
    }
}
