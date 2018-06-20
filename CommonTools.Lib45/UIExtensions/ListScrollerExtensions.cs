using CommonTools.Lib11.DataStructures;
using System.Windows.Controls;
using System.Windows.Media;

namespace CommonTools.Lib45.UIExtensions
{
    public static class ListScrollerExtensions
    {
        public static void ScrollToEnd(this DataGrid dg)
        {
            var count = dg.Items.Count;
            if (count == 0) return;
            //if (VisualTreeHelper.GetChild(dg, 0) is Decorator border)
            //{
            //    var scroll = border.Child as ScrollViewer;
            //    if (scroll != null) scroll.ScrollToEnd();
            //}
            //dg.SelectedIndex = dg.Items.Count - 1;
            dg.ScrollIntoView(dg.Items[count - 1]);
        }



        public static void ScrollToEndOnReplaced<T>(this DataGrid dg)
        {
            var vm = dg.DataContext as UIList<T>;
            vm.ItemsReplaced += (s, e) => dg.ScrollToEnd();
        }
    }
}
