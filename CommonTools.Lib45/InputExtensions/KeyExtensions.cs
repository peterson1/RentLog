using System.Windows.Input;

namespace CommonTools.Lib45.InputExtensions
{
    public static class KeyExtensions
    {
        public static bool IsLetterOrDigit(this Key key)
            => (key >= Key.A       && key <= Key.Z      ) 
            || (key >= Key.D0      && key <= Key.D9     ) 
            || (key >= Key.NumPad0 && key <= Key.NumPad9);
    }
}
