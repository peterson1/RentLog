using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (dg.SelectedItems.Count == 1)
                DeleteSolo(dg, nameGetter, actionBeforeDelete, questionFmt, caption, vm);
            else
                DeleteMany(dg, actionBeforeDelete, caption, vm);
        }


        private static void DeleteSolo<T>(DataGrid dg, Func<T, string> nameGetter, Action<T> actionBeforeDelete, string questionFmt, string caption, UIList<T> vm)
        {
            var item = (T)dg.SelectedItem;
            var msg  = string.Format(questionFmt, nameGetter(item));
            var resp = MessageBox.Show(msg, "   " + caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resp != MessageBoxResult.Yes) return;
            UIThread.Run(() =>
            {
                actionBeforeDelete?.Invoke(item);
                vm.Remove(item);
                vm.RaiseItemDeleted(item);
            });
        }


        private static void DeleteMany<T>(DataGrid dg, Action<T> actionBeforeDelete, string caption, UIList<T> vm)
        {
            var picks = dg.SelectedItems.Cast<T>().ToList();
            var msg   = $"Are you sure you want to delete the selected {picks.Count} items?";
            var resp  = MessageBox.Show(msg, "   " + caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resp != MessageBoxResult.Yes) return;
            UIThread.Run(() =>
            {
                foreach (var item in picks)
                {
                    actionBeforeDelete?.Invoke(item);
                    vm.Remove(item);
                }
                vm.RaiseItemsDeleted(picks);
            });

        }
    }
}
