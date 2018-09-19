using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using RentLog.FilteredLeases.Printing;
using System.Windows;
using System.Windows.Controls;

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
                    => OnPrintClicked();

                VM.ToExcelRequested += (c, d)
                    => FindDataGrid().ExportToExcel();

                VM.PickedListLoaded += (c, d) => FindDataGrid()
                    .EnableOpenCurrent<LeaseBalanceRow>();
            };
        }


        private DataGrid FindDataGrid() 
            => presentr.FindFirstChild<DataGrid>();


        private MainWindowVM VM => DataContext as MainWindowVM;


        private void OnPrintClicked()
        {
            VM.PickedList.IsPrinting = true;
            VM.Show<ColumnsPickerWindow>(false, true);
        }
    }
}
