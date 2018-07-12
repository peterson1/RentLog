using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CommonTools.Lib45.UIExtensions
{
    public static class FocusMoverExtensions
    {
        public static void MoveFocus(this FrameworkElement ctrl, FocusNavigationDirection direction)
            => ctrl.MoveFocus(new TraversalRequest(direction));


        public static void MoveFocusToNext(this FrameworkElement ctrl)
            => ctrl.MoveFocus(FocusNavigationDirection.Next);


        public static void MoveFocusToUp(this FrameworkElement ctrl)
            => ctrl.MoveFocus(FocusNavigationDirection.Up);


        public static void MoveFocusToDown(this FrameworkElement ctrl)
            => ctrl.MoveFocus(FocusNavigationDirection.Down);


        public static void MoveFocusToLeft(this FrameworkElement ctrl)
            => ctrl.MoveFocus(FocusNavigationDirection.Left);


        public static void MoveFocusToRight(this FrameworkElement ctrl)
            => ctrl.MoveFocus(FocusNavigationDirection.Right);


        public static void MoveFocusToNextOnEnterKey(this FrameworkElement ctrl)
        {
            ctrl.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Return)
                {
                    e.Handled = true;
                    ctrl.MoveFocusToNext();
                }
            };
        }


        public static void MoveFocusOnArrowKeys(this FrameworkElement ctrl)
        {
            ctrl.KeyUp += (s, e) =>
            {
                //if (e.Key == Key.Up)
                //    ctrl.MoveFocusToUp();

                //else if (e.Key == Key.Up)
                //    ctrl.MoveFocusToUp();

                //else if (e.Key == Key.Down)
                //    ctrl.MoveFocusToDown();

                //else if (e.Key == Key.Left)
                //    ctrl.MoveFocusToLeft();

                //else if (e.Key == Key.Right)
                //    ctrl.MoveFocusToRight();
                switch (e.Key)
                {
                    case Key.Left:
                        ctrl.MoveFocusToLeft();
                        e.Handled = true;
                        break;
                    case Key.Up:
                        ctrl.MoveFocusToUp();
                        e.Handled = true;
                        break;
                    case Key.Right:
                        ctrl.MoveFocusToRight();
                        e.Handled = true;
                        break;
                    case Key.Down:
                        ctrl.MoveFocusToDown();
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
