using System.Windows.Controls;

namespace CommonTools.Lib45.UIExtensions
{
    public static class TextBoxExtensions
    {
        public static void IncrementInt(this TextBox txt)
        {
            if (!int.TryParse(txt.Text, out int num)) return;
            txt.Text = $"{num + 1}";
        }


        public static void DecrementInt(this TextBox txt)
        {
            if (!int.TryParse(txt.Text, out int num)) return;
            txt.Text = $"{num - 1}";
        }
    }
}
