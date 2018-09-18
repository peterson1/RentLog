using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.UIExtensions;
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
                    => VM.Show<ColumnsPickerWindow>(false, true);

                VM.ToExcelRequested += (c, d)
                    => presentr.FindFirstChild<DataGrid>()
                        .ExportToExcel();
            };
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
