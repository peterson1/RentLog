using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace CommonTools.Lib45.UIExtensions
{
    public static class VisualElementExtensions
    {
        public static IEnumerable<Visual> GetChildren(this Visual parent, bool recurse = true)
        {
            if (parent != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;
                    if (child != null)
                    {
                        yield return child;

                        if (recurse)
                        {
                            foreach (var grandChild in child.GetChildren(true))
                                yield return grandChild;
                        }
                    }
                }
            }
        }


        // https://stackoverflow.com/a/978352/3973863
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
