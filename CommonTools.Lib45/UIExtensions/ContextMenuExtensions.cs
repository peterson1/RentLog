using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.TextLabels;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CommonTools.Lib45.UIExtensions
{
    public static class ContextMenuExtensions
    {
        public static void AddLauncher(this ContextMenu menu, ExeLauncherCommand command)
            => menu.Items.Add(NewLauncher(command));


        public static void Add(this ContextMenu menu, string text)
            => menu.Items.Add(new MenuItem
            {
                Header    = text,
                IsEnabled = false
            });


        public static void Add(this ContextMenu menu, IR2Command command)
        {
            var mnu     = new MenuItem();
            mnu.Command = command;
            var bnd     = new Binding(nameof(IR2Command.CurrentLabel));
            bnd.Source  = command;
            BindingOperations.SetBinding(mnu, MenuItem.HeaderProperty, bnd);
            menu.Items.Add(mnu);
        }


        private static MenuItem NewLauncher(ExeLauncherCommand launcherCmd)
        {
            var mnu         = new MenuItem();
            mnu.DataContext = launcherCmd;
            mnu.Header      = new MenuItemHeader1();
            mnu.Command     = launcherCmd;
            return mnu;
        }
    }
}
