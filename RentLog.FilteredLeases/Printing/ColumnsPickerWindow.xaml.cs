using CommonTools.Lib45.PrintTools;
using CommonTools.Lib45.UIExtensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RentLog.FilteredLeases.Printing
{
    public partial class ColumnsPickerWindow : Window
    {
        public ColumnsPickerWindow()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                var dg = presentr.FindFirstChild<DataGrid>();
                dg.EnableToggledColumns(FindColumnHeaderStyle(dg));

                VM.PickedList.IsPrinting = true;
                VM.PickedList.PrintRequested 
                    += (c, d) => PrintCurrentList();
            };
        }


        private void PrintCurrentList()
        {
            var dg = presentr.FindFirstChild<DataGrid>();
            dg.AskToPrint(VM.PickedFilterName);
            VM.PickedList.IsPrinting = false;
            Close();
        }


        private Style FindColumnHeaderStyle(DataGrid dg)
            => dg?.Style?.Resources?.Values?
                .OfType<Style>()?.FirstOrDefault();


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
