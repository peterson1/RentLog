using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    public partial class UncollectedsUI : UserControl
    {
        public UncollectedsUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.ItemsList.ItemOpened  += async (c, d) => await TryFocusOnTable();
                VM.Main.RefreshCompleted += async (c, d) => await TryFocusOnTable();
            };
        }


        private async Task TryFocusOnFilter()
        {
            if (!filtr.IsVisible) return;
            await Task.Delay(100);
            filtr.txt1.Focus();
        }


        private async Task TryFocusOnTable()
        {
            if (!tbl.IsVisible) return;

            await TryFocusOnFilter();
            filtr.txt1.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            await Task.Delay(100);
            await TryFocusOnFilter();
            filtr.txt1.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            //tbl.dg.Focus();
            //if (tbl.dg.HasItems)
            //{
            //    var row = tbl.dg.ItemContainerGenerator.ContainerFromItem(tbl.dg.SelectedItem) as DataGridRow;
            //    row?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            //}
            //TryFocusOnFilter();

            //tbl.dg.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
        }


        private UncollectedsVM VM => DataContext as UncollectedsVM;
    }
}
