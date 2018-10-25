using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ApplicationTools;
using CommonTools.Lib45.FileSystemTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.ImportBYF.ByfQueries;
using RentLog.ImportBYF.CommonJsonComparer;
using RentLog.ImportBYF.Converters;
using RentLog.ImportBYF.Converters.BankAccountConverters;
using RentLog.ImportBYF.Converters.CollectorConverters;
using RentLog.ImportBYF.Converters.GLAccountConverters;
using RentLog.ImportBYF.Converters.LeaseConverters;
using RentLog.ImportBYF.Converters.SectionConverters;
using RentLog.ImportBYF.Converters.StallConverters;
using RentLog.ImportBYF.DailyTransactions;
using RentLog.ImportBYF.RntQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Environment;

namespace RentLog.ImportBYF
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Import BYF";

        private Dictionary<int, Func<ComparisonsListBase>> _enlisteds = new Dictionary<int, Func<ComparisonsListBase>>();


        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            DailyTxns = new DailyTransactionsVM(this);
            Enlist("Leases"    , () => new LeaseConverter1      (this));
            Enlist("Stalls"    , () => new StallConverter1      (this));
            Enlist("Sections"  , () => new SectionConverter1    (this));
            Enlist("Collectors", () => new CollectorConverter1  (this));
            Enlist("Bank Accts", () => new BankAccountConverter1(this));
            Enlist("GL Accts"  , () => new GLAccountConverter1  (this));
        }


        public RntCache             RntCache         { get; } = new RntCache();
        public ByfCache             ByfCache         { get; } = new ByfCache();
        public DailyTransactionsVM  DailyTxns        { get; }
        public UIList<string>       ListNames        { get; } = new UIList<string>();
        public int                  PickedListIndex  { get; set; } = -1;
        public ComparisonsListBase  PickedList       { get; private set; }
        public string               PickedListName   => ListNames[PickedListIndex];


        public string CacheDir => SpecialFolder.LocalApplicationData.Path("BasicAuthBulkCacheReader");


        protected override void OnWindowLoaded()
        {
            if (PickedListIndex == -1)
                PickedListIndex = 0;

            DailyTxns.RefreshCmd.ExecuteIfItCan();
        }


        public void OnPickedListIndexChanged()
        {
            PickedList = null;
            PickedList = _enlisteds[PickedListIndex]();
            ClickRefresh();
        }


        protected override async Task OnRefreshClickedAsync()
        {
            if (PickedList == null) return;
            StartBeingBusy($"Loading {PickedListName} ...");
            await Task.Delay(1);
            await Task.Run(() =>
                Parallel.Invoke(() => PickedList.ReloadList(),
                                () => RntCache.RefillFrom(AppArgs),
                                () => ByfCache.RefillFromCache(CacheDir)));
            StopBeingBusy();
        }


        private LeaseDTO CastBYF(ReportModels.Lease byf) => new LeaseDTO
        {
            ContractStart = byf.ContractStart,
            ContractEnd = byf.ContractEnd,
        };


        private void Enlist<T>(string label, Func<T> constructor)
            where T : ComparisonsListBase
        {
            _enlisteds.Add(_enlisteds.Count, constructor);
            ListNames.Add(label);
            App.Current.SetTemplate<T, JsComparerTable>();
        }
    }
}
