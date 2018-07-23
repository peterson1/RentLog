using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.Cashiering.BankDeposits;
using RentLog.Cashiering.CashierCollections;
using RentLog.Cashiering.MainToolbar;
using RentLog.Cashiering.OtherCollections;
using RentLog.Cashiering.SectionTabs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        private string UserTask => CanReview ? "Reviewing" : "Encoding";
        public override string SubAppName => $"Cashiering  :  {UserTask} Collections for {Date:MMMM d, yyyy}";


        public MainWindowVM(DateTime unclosedDate, ITenantDBsDir tenantDBsDir, bool clickRefresh = true) : base(tenantDBsDir)
        {
            Date      = unclosedDate;
            CanReview = AppArgs.CanPostAndClose(false);
            CanEncode = AppArgs.CanEncodeCollections(false);
            if (!IsPrivilegedUser()) return;

            ColxnsDB = CheckIfDbExists(Date);
            if (ColxnsDB == null) return;

            CashierColxns   = new CashierColxnsVM   (this);
            OtherColxns     = new OtherColxnsVM     (this);
            BankDeposits    = new BankDepositsVM    (this);
            ApprovalAwaiter = new ApprovalRequesterVM(this);
            PostAndClose    = new PostAndCloseVM    (this);
            NextDayOpener   = new NextDayOpenerVM   (this);

            if (clickRefresh) ClickRefresh();
            SetCaption("");
        }


        public DateTime              Date              { get; }
        public ICollectionsDB        ColxnsDB          { get; }
        public bool                  CanReview         { get; }
        public bool                  CanEncode         { get; }
        public UIList<SectionTabVM>  SectionTabs       { get; } = new UIList<SectionTabVM>();
        public CashierColxnsVM       CashierColxns     { get; }
        public OtherColxnsVM         OtherColxns       { get; }
        public BankDepositsVM        BankDeposits      { get; }
        public PostAndCloseVM        PostAndClose      { get; }
        public NextDayOpenerVM       NextDayOpener     { get; }
        public ApprovalRequesterVM   ApprovalAwaiter   { get; }
        public int                   CurrentTabIndex   { get; set; }


        public void OnCurrentTabIndexChanged()
            => AppArgs.CurrentSection = CurrentTabIndex == -1 ? null 
                                      : SectionTabs[CurrentTabIndex].Section;


        private bool IsPrivilegedUser()
        {
            if (CanReview || CanEncode) return true;
            WhyShouldClose = "User cannot Encode and cannot Review";
            ShouldClose = true;
            return false;
        }


        private ICollectionsDB CheckIfDbExists(DateTime unclosedDate)
        {
            var db = AppArgs.Collections.For(unclosedDate);
            if (db != null) return db;

            if (CanEncode)
                return AppArgs.Collections.CreateFor(unclosedDate);

            ShouldClose = true;
            WhyShouldClose = "Nothing to review yet.";
            return null;
        }


        protected override async Task OnRefreshClickedAsync()
        {
            await NextDayOpener.RunIfNeeded();
            Parallel.Invoke(() => ReloadAllSectionTabs(),
                            () => CashierColxns.ReloadFromDB(),
                            () => OtherColxns.ReloadFromDB(),
                            () => BankDeposits.ReloadFromDB());
            PostAndClose.UpdateTotals();
        }


        //public async Task ReloadCurrentSectionTab()
        //{
        //    if (IsBusy) return;
        //    StartBeingBusy($"Reloading “{AppArgs.CurrentSection}”...");
        //    await Task.Run(() =>
        //    {
        //        SectionTabs[CurrentTabIndex].ReloadAll();
        //        PostAndClose.UpdateTotals();
        //    });
        //    StopBeingBusy();
        //}
        //public SectionTabVM CurrentSectionTab => SectionTabs[CurrentTabIndex];


        private void ReloadAllSectionTabs()
        {
            var lastIndx = CurrentTabIndex;
            var list     = new List<SectionTabVM>();
            var all      = AppArgs.MarketState.Sections.GetAll();

            foreach (var sec in all)
                list.Add(new SectionTabVM(sec, this));

            AsUI(() => SectionTabs.SetItems(list));
            AppArgs.CurrentSection = SectionTabs.FirstOrDefault()?.Section;
            CurrentTabIndex = SectionTabs.Any() ? lastIndx : -1;

            Parallel.ForEach(SectionTabs, _ => _.ReloadAll());
        }


        public override async Task OnWindowClosing(CancelEventArgs cancelEvtArgs)
        {
            if (!CanEncode) return;
            cancelEvtArgs.Cancel = true;
            StartBeingBusy("Updating Vacants & Uncollecteds ...");
            await Task.Run(() => UpdateDatabasesBeforeExit());

            if (PostAndClose.IsCashierSubmitting)
            {
                PostAndClose.IsCashierSubmitting = false;
                await ApprovalAwaiter.WaitForApproval();
                return;
            }
            await Task.Delay(1000);
            StopBeingBusy();
            Application.Current?.Shutdown();
        }


        private void UpdateDatabasesBeforeExit()
        {
            ColxnsDB.TakeSectionsSnapshot
                (AppArgs.MarketState.Sections.GetAll());

            ColxnsDB.TakeCollectorsSnapshot
                (AppArgs.MarketState.Collectors.GetAll());

            ColxnsDB.VacantStalls.UpdateAllLists(AppArgs.MarketState);

            foreach (var secTab in SectionTabs)
                secTab.Uncollecteds.PersistUIList(true);
        }
    }
}
