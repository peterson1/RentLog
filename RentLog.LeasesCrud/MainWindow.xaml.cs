using CommonTools.Lib45.ThreadTools;
using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using RentLog.LeasesCrud.LeasesList;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RentLog.LeasesCrud
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                tabs.SelectionChanged += (c, d) => SetCurrentLists();
                SetCurrentLists();
                
                actives  .tbl.dg.EnableOpenCurrent<LeaseBalanceRow>();
                inactives.tbl.dg.EnableOpenCurrent<LeaseBalanceRow>();
            };
        }

        private void SetCurrentLists()
        {
            VM.CurrentRows     = CurrentTabVM as IEnumerable<LeaseBalanceRow>;
            VM.CurrentIsActive = CurrentTabVM is ActiveLeasesVM;
            VM.CurrentTable    = VM.CurrentIsActive ? actives.tbl.dg 
                                                    : inactives.tbl.dg;
        }


        private MainWindowVM  VM           => DataContext as MainWindowVM;
        private TabItem       CurrentTab   => tabs.SelectedItem as TabItem;
        private object        CurrentTabVM => CurrentTab.DataContext;
    }
}
