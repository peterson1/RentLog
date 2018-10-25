using CommonTools.Lib11.ExceptionTools;
using PropertyChanged;
using RentLog.ChequeVouchers.AllChequeVouchers;
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
    public enum MainTabs
    {
        CheckVouchers   = 0,
        DcdrReport      = 1,
        JournalVouchers = 2
    }


    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Cheque Vouchers  |  DCDR";


        public MainWindowVM(ITenantDBsDir tenantDBsDir, bool clickRefresh = true) : base(tenantDBsDir)
        {
            DateRange      = new DateRangePickerVM(this);
            BankAcctPicker = new BankAccountPickerVM(this, clickRefresh);
            VoucherReqs    = new VoucherReqsTabVM(this);
            DcdrReport     = new DcdrTabVM(this);
            Journals       = new JournalsTabVM(this);
            AllChkVouchers = new AllChequeVouchersVM(this);

            PrintCmd.SetLabel("Print “Pending Checks” List");
        }


        public BankAccountPickerVM  BankAcctPicker  { get; }
        public DateRangePickerVM    DateRange       { get; }
        public VoucherReqsTabVM     VoucherReqs     { get; }
        public DcdrTabVM            DcdrReport      { get; }
        public JournalsTabVM        Journals        { get; }
        public AllChequeVouchersVM  AllChkVouchers  { get; }
        public int                  SelectedIndex   { get; set; }
        public MainTabs             SelectedTab
        {
            get => (MainTabs)SelectedIndex;
            set => SelectedIndex = (int)value;
        }


        public void OnSelectedIndexChanged()
        {
            DateRange.IsVisible = SelectedTab > MainTabs.CheckVouchers;
            ClickRefresh();
        }


        protected override async Task OnRefreshClickedAsync()
        {
            StartBeingBusy($"Loading Cheque Vouchers ...");

            await Task.Run(PickRefreshJob());

            SetCaption($"for “{AppArgs.CurrentBankAcct}”");
            StopBeingBusy();
        }


        private Action PickRefreshJob() { switch (SelectedTab)
        {
            case MainTabs.CheckVouchers  : return () => VoucherReqs.ReloadAll();
            case MainTabs.DcdrReport     : return () => DcdrReport.PassbookRows.ReloadFromDB();
            case MainTabs.JournalVouchers: return () => Journals.JournalRows.ReloadFromDB();
            default: throw Bad.Arg("MainTabs(enum)", SelectedIndex);
        }}
    }
}
