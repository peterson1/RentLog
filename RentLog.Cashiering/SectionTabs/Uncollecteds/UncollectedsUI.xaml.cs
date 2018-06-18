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
                VM.ItemsList.ItemOpened  += (c, d) => TryFocusOnFilter();
                VM.Main.RefreshCompleted += (c, d) => TryFocusOnFilter();
            };
        }


        private void TryFocusOnFilter()
        {
            if (!filtr.IsVisible) return;
            filtr.txt1.Focus();
        }


        private UncollectedsVM VM => DataContext as UncollectedsVM;
    }
}
