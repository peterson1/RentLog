using CommonTools.Lib45.InputExtensions;
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
                tbl.Loaded += Tbl_Loaded;
            };
        }


        private void Tbl_Loaded(object sender, System.Windows.RoutedEventArgs args)
        {
            tbl.dg.PreviewKeyDown += async (s, e) =>
            {
                if (e.Key.IsLetterOrDigit()
                && !Keyboard.IsKeyDown(Key.LeftCtrl))
                    await TryFocusOnFilter();
            };
        }


        private async Task TryFocusOnFilter()
        {
            if (!filtr.IsVisible) return;
            filtr.txt1.Focus();
            await Task.Delay(100);
            filtr.txt1.Focus();
        }


        private async Task TryFocusOnTable()
        {
            if (!tbl.IsVisible) return;

            await TryFocusOnFilter();
            filtr.txt1.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            await TryFocusOnFilter();
            filtr.txt1.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }


        private UncollectedsVM VM => DataContext as UncollectedsVM;
    }
}
