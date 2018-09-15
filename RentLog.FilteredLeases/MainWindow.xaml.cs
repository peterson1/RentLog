using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommonTools.Lib45.PrintTools;
using CommonTools.Lib45.UIExtensions;

namespace RentLog.FilteredLeases
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.PrintClicked += (c, d) 
                    => PrintCurrentList();
            };
        }


        private void PrintCurrentList()
        {
            var allDGs = presentr.FindVisualChildren<DataGrid>();
            var dg     = allDGs.Single();
            dg.AskToPrint(VM.PickedFilterName);
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
