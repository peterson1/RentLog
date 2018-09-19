using CommonTools.Lib45.PrintTools;
using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RentLog.FilteredLeases.Printing
{
    public partial class ColumnsPickerWindow : Window
    {
        public ColumnsPickerWindow()
        {
            InitializeComponent();
            Loaded += async (a, b) =>
            {
                await Task.Delay(500);

                var dg = presentr.FindFirstChild<DataGrid>();
                dg.EnableToggledColumns(FindColumnHeaderStyle(dg));
                dg.EnableOpenCurrent<LeaseBalanceRow>();
                VM.PickedList.PrintRequested 
                    += (c, d) => PrintCurrentList();
            };

            Closing += (a, b) 
                => VM.PickedList.IsPrinting = false;
        }


        private void PrintCurrentList()
        {
            var dg = presentr.FindFirstChild<DataGrid>();
            dg.AskToPrint(VM.PickedList.TopLeftText,
                          VM.AppArgs.MarketState.BranchName, 
                          VM.PickedList.TopRightText);
            Close();
        }


        private Style FindColumnHeaderStyle(DataGrid dg)
            => dg?.Style?.Resources?.Values?
                .OfType<Style>()?.FirstOrDefault();


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
