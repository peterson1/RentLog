using CommonTools.Lib11.StringTools;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CommonTools.Lib45.ValueConverters
{
    public abstract class FirstNCharsConverterBase : IValueConverter
    {
        public abstract int CharacterCount { get; }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;
            if (text.IsBlank()) return "";

            var oneLine = text.Replace(L.f, " ")
                              .Replace(L.F, " ");

            return oneLine.Length < CharacterCount ? oneLine 
                 : oneLine.Substring(0, CharacterCount);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw new NotImplementedException();
    }


    public class First30CharsConverter : FirstNCharsConverterBase
    {
        public override int CharacterCount => 30;
    }


    public class First60CharsConverter : FirstNCharsConverterBase
    {
        public override int CharacterCount => 60;
    }
}
