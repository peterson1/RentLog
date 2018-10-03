using System.Windows;

namespace CommonTools.Lib45.ApplicationTools
{
    public static class DataTemplateRegisterer
    {
        public static void SetTemplate<TData, TUI>(this Application app)
        {
            var dict = app.Resources;
            var key  = new DataTemplateKey(typeof(TData));
            var elm  = new FrameworkElementFactory(typeof(TUI));
            var tpl  = new DataTemplate { VisualTree = elm };
            dict.Add(key, tpl);
        }
    }
}
