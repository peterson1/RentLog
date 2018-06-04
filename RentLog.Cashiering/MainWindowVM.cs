using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.Cashiering.BankDeposits;
using RentLog.Cashiering.CashierCollections;
using RentLog.Cashiering.MainToolbar;
using RentLog.Cashiering.OtherCollections;
using RentLog.Cashiering.SectionTabs;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RentLog.Cashiering
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => $"Cashiering  :  Collections for {Date:MMMM d, yyyy}";
        private ICollectionsDB _colxnsDB;


        public MainWindowVM(DateTime date, ITenantDBsDir tenantDBsDir, bool clickRefresh = true) : base(tenantDBsDir)
        {
            Date          = date;
            _colxnsDB     = AppArgs.Collections.For(Date);
            if (_colxnsDB == null)
            {
                MessageBox.Show("Nothing to review yet.");
                Application.Current.Shutdown();
                return;
            }
            CashierColxns = new CashierColxnsVM(_colxnsDB.CashierColxns, AppArgs);
            OtherColxns   = new OtherColxnsVM(_colxnsDB.OtherColxns, AppArgs);
            BankDeposits  = new BankDepositsVM(_colxnsDB.BankDeposits, AppArgs);
            PostAndClose  = new PostAndCloseVM(this);
            if (clickRefresh) ClickRefresh();
            SetCaption("");
        }


        public DateTime              Date             { get; }
        public UIList<SectionTabVM>  SectionTabs      { get; } = new UIList<SectionTabVM>();
        public CashierColxnsVM       CashierColxns    { get; }
        public OtherColxnsVM         OtherColxns      { get; }
        public BankDepositsVM        BankDeposits     { get; }
        public PostAndCloseVM        PostAndClose     { get; }


        protected override void OnRefreshClicked()
        {
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
                list.Add(new SectionTabVM(sec, _colxnsDB, AppArgs));

            AsUI(() => SectionTabs.SetItems(list));

            foreach (var tab in SectionTabs)
                tab.ReloadAll();
        }
    }
}
