using CommonTools.Lib45.ValidationRules;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CommonTools.Lib45.UIExtensions
{
    public static class UIValidationExtensions
    {
        //https://stackoverflow.com/a/4650392/3973863
        public static bool AllFieldsValid(this DependencyObject obj)
            => !Validation.GetHasError(obj)
            && LogicalTreeHelper.GetChildren(obj)
                                .OfType<DependencyObject>()
                                .All(AllFieldsValid);


        public static void ValidateNonBlank(this TextBox txt)
        {
            if (txt.Tag == null) return;
            var b = new Binding(txt.Tag.ToString());
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            b.ValidationRules.Add(new NotBlankValidationRule());
            txt.SetBinding(TextBox.TextProperty, b);
        }
    }
}
