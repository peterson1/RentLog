using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CommonTools.Lib45.UIExtensions
{
    public static class DataGridColumnToggler
    {
        public static void EnableToggledColumns(this DataGrid dg, Style columnHeaderStyle)
            => dg.ColumnHeaderStyle = CreateColumnHeaderStyle(dg, columnHeaderStyle, DataGridLengthUnitType.Auto);


        private static Style CreateColumnHeaderStyle(DataGrid dg, Style columnHeaderStyle, DataGridLengthUnitType colWidthType)
        {
            var newStyle = new Style(typeof(DataGridColumnHeader), columnHeaderStyle);
            var ctxMenuKey = $"ctxMenu_{Guid.NewGuid().ToString("N")}";
            var ctxMenu = CreateContextMenu(dg, colWidthType);
            newStyle.Resources.Add(ctxMenuKey, ctxMenu);
            newStyle.Setters.Add(ContextMenuSetter(ctxMenu));
            return newStyle;
        }


        private static Setter ContextMenuSetter(ContextMenu ctxMenu)
            => new Setter(DataGridColumnHeader.ContextMenuProperty, ctxMenu);


        private static ContextMenu CreateContextMenu(DataGrid dg, DataGridLengthUnitType colWidthType)
        {
            var cm = new ContextMenu();

            foreach (var col in dg.Columns)
            {
                var mnu = new MenuItem();
                mnu.Header = col.Header;
                mnu.IsCheckable = true;
                mnu.IsChecked = col.Visibility == Visibility.Visible;
                mnu.Click += (s, e) => col.Visibility = mnu.IsChecked
                            ? Visibility.Visible : Visibility.Collapsed;
                cm.Items.Add(mnu);
                col.Width = new DataGridLength(0, colWidthType);
            }

            return cm;
        }
    }
}
