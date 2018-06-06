using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib45.UIExtensions
{
    public static class ConfirmToDeleteExtensions
    {
        public static void ConfirmToDelete<T>(this DataGrid dg,
            Func<T, string> nameGetter, 
            Action<T> actionBeforeDelete = null,
            string questionFmt = "Are you sure you want to delete the entry for “{0}”?",
            string caption = "Confirm to Delete")
        {
            dg.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Key.Delete)
                    RaiseDeletedIfConfirmed(dg, nameGetter, 
                        actionBeforeDelete, questionFmt, caption);
            };
        }

        private static void RaiseDeletedIfConfirmed<T>(DataGrid dg, Func<T, string> nameGetter, Action<T> actionBeforeDelete, string questionFmt, string caption)
        {
            if (dg.SelectedIndex == -1) return;
            var vm = dg.DataContext as UIList<T>;
            //var item = vm.CurrentItem;
            var item = (T)dg.SelectedItem;
            var msg = string.Format(questionFmt, nameGetter(item));
            var resp = MessageBox.Show(msg , "   " + caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resp == MessageBoxResult.Yes)
            {
                UIThread.Run(() =>
                {
                    actionBeforeDelete?.Invoke(item);
                    vm.Remove(item);
                    vm.RaiseItemDeleted(item);
                });
            }
        }
    }
}
