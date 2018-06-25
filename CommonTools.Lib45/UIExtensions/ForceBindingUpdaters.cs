using System.Windows;
using System.Windows.Controls;

namespace CommonTools.Lib45.UIExtensions
{
    public static class ForceBindingUpdaters
    {
        public static void ForceUpdateTarget(this TextBox ctrl)
            => ctrl.ForceUpdateTarget(TextBox.TextProperty);


        public static void ForceUpdateTarget(this TextBlock ctrl)
            => ctrl.ForceUpdateTarget(TextBlock.TextProperty);


        public static void ForceUpdateTarget(this FrameworkElement ctrl, DependencyProperty dependencyProperty)
            => ctrl.GetBindingExpression(dependencyProperty).UpdateTarget();
    }
}
