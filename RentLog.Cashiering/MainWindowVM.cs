using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.Cashiering.BankDeposits;
using RentLog.Cashiering.CashierCollections;
using RentLog.Cashiering.MainToolbar;
using RentLog.Cashiering.OtherCollections;
using RentLog.Cashiering.SectionTabs;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.Cashiering
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => $"Cashiering  :  Collections for {Date:MMMM d, yyyy}";
        private ICollectionsDB _colxnsDB;


        public MainWindowVM(DateTime date, AppArguments appArguments, bool clickRefresh = true) : base(appArguments)
        {
            Date          = date;
            _colxnsDB     = AppArgs.Collections.For(Date);
            MainToolbar   = new MainToolbarVM(this);
            CashierColxns = new CashierColxnsVM(_colxnsDB.CashierColxns, AppArgs);
            OtherColxns   = new OtherColxnsVM(_colxnsDB.OtherColxns, AppArgs);
            BankDeposits  = new BankDepositsVM(_colxnsDB.BankDeposits, AppArgs);
            if (clickRefresh) ClickRefresh();
            SetCaption("");
        }


        public DateTime              Date          { get; }
        public MainToolbarVM         MainToolbar   { get; }
        public UIList<SectionTabVM>  SectionTabs   { get; } = new UIList<SectionTabVM>();
        public CashierColxnsVM       CashierColxns { get; }
        public OtherColxnsVM         OtherColxns   { get; }
        public BankDepositsVM        BankDeposits  { get; }

        public decimal TotalDeposits    => BankDeposits.TotalSum;
        public decimal TotalCollections => SectionTabs.Sum(_ => _.SectionTotal)
                                         + CashierColxns.TotalSum
                                         + OtherColxns.TotalSum;


        protected override void OnRefreshClicked()
        {
            ReloadSectionTabs();
            CashierColxns.ReloadFromDB();
            OtherColxns.ReloadFromDB();
            BankDeposits.ReloadFromDB();
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
