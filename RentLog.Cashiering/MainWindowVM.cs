﻿using CommonTools.Lib11.DataStructures;
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
using System.Threading.Tasks;

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

            CashierColxns = new CashierColxnsVM (this);
            OtherColxns   = new OtherColxnsVM   (this);
            BankDeposits  = new BankDepositsVM  (this);
            PostAndClose  = new PostAndCloseVM  (this);
            NextDayOpener = new NextDayOpenerVM (this);
            if (clickRefresh) ClickRefresh();
            SetCaption("");
        }


        public DateTime              Date             { get; }
        public ICollectionsDB        ColxnsDB         { get; }
        public bool                  CanReview        { get; }
        public bool                  CanEncode        { get; }
        public UIList<SectionTabVM>  SectionTabs      { get; } = new UIList<SectionTabVM>();
        public CashierColxnsVM       CashierColxns    { get; }
        public OtherColxnsVM         OtherColxns      { get; }
        public BankDepositsVM        BankDeposits     { get; }
        public PostAndCloseVM        PostAndClose     { get; }
        public NextDayOpenerVM       NextDayOpener    { get; set; }


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
            ReloadSectionTabs();
            CashierColxns.ReloadFromDB();
            OtherColxns  .ReloadFromDB();
            BankDeposits .ReloadFromDB();
            PostAndClose .UpdateTotals();
        }


        private void ReloadSectionTabs()
        {
            var list = new List<SectionTabVM>();
            var all  = AppArgs.MarketState.Sections.GetAll();

            foreach (var sec in all)
                list.Add(new SectionTabVM(sec, this));

            AsUI(() => SectionTabs.SetItems(list));

            foreach (var tab in SectionTabs)
                tab.ReloadAll();
        }


        protected override void OnWindowClosing(CancelEventArgs cancelEvtArgs)
        {
            if (CanEncode) UpdateDatabasesBeforeExit();
        }


        private void UpdateDatabasesBeforeExit()
        {
            ColxnsDB.VacantStalls.UpdateAllLists(AppArgs.MarketState);

            foreach (var secTab in SectionTabs)
                secTab.Uncollecteds.PersistUIList(true);
        }
    }
}
