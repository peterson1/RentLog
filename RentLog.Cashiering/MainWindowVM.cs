using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.Cashiering.BankDeposits;
using RentLog.Cashiering.CashierCollections;
using RentLog.Cashiering.MainToolbar;
using RentLog.Cashiering.OtherCollections;
using RentLog.Cashiering.SectionTabs;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using System;
using System.Linq;

namespace RentLog.Cashiering
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => $"Cashiering  :  Collections for {Date:MMMM d, yyyy}";


        public MainWindowVM(DateTime date, AppArguments appArguments, bool clickRefresh = true) : base(appArguments)
        {
            Date          = date;
            var colxnsDB  = AppArgs.Collections.For(Date);
            MainToolbar   = new MainToolbarVM(this);
            BankDeposits  = new BankDepositsVM(colxnsDB.BankDeposits, AppArgs);
            CashierColxns = new CashierColxnsVM(colxnsDB.CashierColxns, AppArgs);
            OtherColxns   = new OtherColxnsVM(colxnsDB.OtherColxns, AppArgs);
            if (clickRefresh) ClickRefresh();
            SetCaption("");
        }


        public DateTime              Date          { get; }
        public MainToolbarVM         MainToolbar   { get; }
        public UIList<SectionTabVM>  SectionTabs   { get; } = new UIList<SectionTabVM>();
        public BankDepositsVM        BankDeposits  { get; }
        public CashierColxnsVM       CashierColxns { get; }
        public OtherColxnsVM         OtherColxns   { get; }


        protected override void OnRefreshClicked()
        {
            var tabs = AppArgs.MarketState.Sections.GetAll().Select(_ => new SectionTabVM(_, AppArgs));
            AsUI(() => SectionTabs.SetItems(tabs));
            BankDeposits.ReloadFromDB();
            CashierColxns.ReloadFromDB();
            OtherColxns.ReloadFromDB();
        }
    }
}
