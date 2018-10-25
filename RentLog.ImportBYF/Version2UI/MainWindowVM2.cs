using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.RntQueries;
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
            AppArgs.Credentials.HumanName = "Migrator";
            AppArgs.Credentials.Roles     = "Admin";
            SetCaption(AppArgs.Credentials.NameAndRole);

            ByfServer       = new ByfServerVM(this);
            MasterData      = new MasterDataPaneVM(this);
            TransactionData = new TransactionDataPaneVM(this);
        }


        public ByfCache              ByfCache        { get; } = new ByfCache();
        public RntCache              RntCache        { get; } = new RntCache();
        public ByfServerVM           ByfServer       { get; }
        public MasterDataPaneVM      MasterData      { get; }
        public TransactionDataPaneVM TransactionData { get; }


        protected override async void OnWindowLoaded()
        {
            RntCache.RefillFrom(AppArgs);
            await ByfServer.SetURL(Default.ServerURL);
            if (ByfServer.IsOnline)
                await ByfCache.RefillFromServer(ByfServer);
        }
    }
}
