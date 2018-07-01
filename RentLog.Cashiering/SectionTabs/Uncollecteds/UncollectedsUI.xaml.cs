using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    public partial class UncollectedsUI : UserControl
    {
        public UncollectedsUI()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.ItemsList.ItemOpened  += (c, d) => TryFocusOnTable();
                VM.Main.RefreshCompleted += (c, d) => TryFocusOnTable();
            };
        }


        private void TryFocusOnFilter()
        {
            if (!filtr.IsVisible) return;
            filtr.txt1.Focus();
        }


        private void TryFocusOnTable()
        {
            if (!tbl.IsVisible) return;
            tbl.dg.Focus();
        }


        private UncollectedsVM VM => DataContext as UncollectedsVM;
    }
}
