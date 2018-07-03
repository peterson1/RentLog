using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib45.UIExtensions
{
    public static class FocusMoverExtensions
    {
        public static void MoveFocusToNext(this FrameworkElement ctrl)
            => ctrl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));


        public static void MoveFocusToNextOnEnterKey(this TextBox txtbox)
        {
            txtbox.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Return)
                    txtbox.MoveFocusToNext();
            };
        }
    }
}
