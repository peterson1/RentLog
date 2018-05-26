using System;
using System.Windows;

namespace CommonTools.Lib45.ThreadTools
{
    public static class UIThread
    {
        public static void Run(Action action)
        {
            if (Application.Current == null)
                action.Invoke();
            else
                Application.Current.Dispatcher.Invoke(action);
        }
    }
}
