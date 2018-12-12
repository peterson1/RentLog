using CommonTools.Lib11.StringTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.RntQueries;
using RentLog.ImportBYF.Version2UI.BankMemosPane;
using RentLog.ImportBYF.Version2UI.CheckVouchersPane;
using RentLog.ImportBYF.Version2UI.JournalVouchersPane;
using RentLog.ImportBYF.Version2UI.LeaseBalancesPane;
using RentLog.ImportBYF.Version2UI.MasterDataPane;
using RentLog.ImportBYF.Version2UI.TransactionDataPane;
using static RentLog.ImportBYF.Properties.Settings;

namespace RentLog.ImportBYF.Version2UI
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM2 : BrandedWindowBase
    {
        public override string SubAppName => "Import BYF";


        public MainWindowVM2(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            SetBranchAndSystemNames();
            AppArgs.Credentials.HumanName = "Migrator";
            AppArgs.Credentials.Roles = "Admin";
            SetCaption(AppArgs.Credentials.NameAndRole);

            ByfServer       = new ByfServerVM(this);
            MasterData      = new MasterDataPaneVM(this);
            TransactionData = new TransactionDataPaneVM(this);
            //LeaseBalances   = new LeaseBalancesPaneVM(this);
            CheckVouchers   = new CheckVouchersPaneVM(this);
            BankMemos       = new BankMemosPaneVM(this);
            JournalVouchers = new JournalVouchersPaneVM(this);
        }


        public ByfCache               ByfCache         { get; } = new ByfCache();
        public RntCache               RntCache         { get; } = new RntCache();
        public ByfServerVM            ByfServer        { get; }
        public MasterDataPaneVM       MasterData       { get; }
        public TransactionDataPaneVM  TransactionData  { get; }
        public LeaseBalancesPaneVM    LeaseBalances    { get; }
        public CheckVouchersPaneVM    CheckVouchers    { get; }
        public BankMemosPaneVM        BankMemos        { get; }
        public JournalVouchersPaneVM  JournalVouchers  { get; }


        private void SetBranchAndSystemNames()
        {
            var mkt = AppArgs.MarketState;

            if (mkt.BranchName.IsBlank())
                mkt.BranchName = "branch-name-goes-here";

            if (mkt.SystemName.IsBlank())
                mkt.SystemName = "system-name-goes-here";
        }


        protected override async void OnWindowLoaded()
        {
            RntCache.RefillFrom(AppArgs);
            await ByfServer.SetURL(Default.ServerURL);
            if (ByfServer.IsOnline)
                await ByfCache.RefillFromServer(ByfServer);
        }
    }
}
