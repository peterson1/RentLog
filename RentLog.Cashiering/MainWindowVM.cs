using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.Cashiering.BankDeposits;
using RentLog.Cashiering.CashierCollections;
using RentLog.Cashiering.OtherCollections;
using RentLog.Cashiering.SectionTabs;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.Cashiering
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => $"Cashiering  :  Collections for {Date:MMMM d, yyyy}";
        private ICollectionsDB _colxnsDB;


        public MainWindowVM(DateTime date, AppArguments appArguments, bool clickRefresh = true) : base(appArguments)
        {
            Date            = date;
            _colxnsDB       = AppArgs.Collections.For(Date);
            CashierColxns   = new CashierColxnsVM(_colxnsDB.CashierColxns, AppArgs);
            OtherColxns     = new OtherColxnsVM(_colxnsDB.OtherColxns, AppArgs);
            BankDeposits    = new BankDepositsVM(_colxnsDB.BankDeposits, AppArgs);
            PostAndCloseCmd = R2Command.Relay(DoPostAndClose, _ => IsBalanced, "Post & Close");
            if (clickRefresh) ClickRefresh();
            SetCaption("");
        }


        public DateTime              Date             { get; }
        public UIList<SectionTabVM>  SectionTabs      { get; } = new UIList<SectionTabVM>();
        public CashierColxnsVM       CashierColxns    { get; }
        public OtherColxnsVM         OtherColxns      { get; }
        public BankDepositsVM        BankDeposits     { get; }
        public IR2Command            PostAndCloseCmd  { get; }

        public decimal TotalDeposits    { get; private set; }
        public decimal TotalCollections { get; private set; }
        public decimal TotalDifference  { get; private set; }
        public bool    IsBalanced       { get; private set; }


        protected override void OnRefreshClicked()
        {
            ReloadSectionTabs();
            CashierColxns.ReloadFromDB();
            OtherColxns  .ReloadFromDB();
            BankDeposits .ReloadFromDB();
            TotalDeposits    = BankDeposits.TotalSum;
            TotalCollections = SectionTabs.Sum(_ => _.SectionTotal)
                             + CashierColxns.TotalSum
                             + OtherColxns.TotalSum;
            TotalDifference  = Math.Abs(TotalDeposits - TotalCollections);
            IsBalanced       = TotalDeposits == TotalCollections;
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


        private void DoPostAndClose()
            => Alert.Confirm("Please Confirm",
                "Are you sure you want to close this day and open the next?", async () =>
                {
                    StartBeingBusy("Posting and Closing ...");

                    await Task.Delay(1000 * 5);

                    AsUI(() => MessageBox.Show($"Successfully posted collections for {Date:d-MMM-yyyy}{L.F}"
                             + $"The next market day [{Date.AddDays(1):d-MMM-yyyy}] is now open for encoding.",
                            "   Operation Successful", MessageBoxButton.OK, MessageBoxImage.Information));
                    CloseWindow();
                });
    }
}
