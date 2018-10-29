using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using System;

namespace CommonTools.Lib11.DynamicTools
{
    public static class As
    {
        public static string Text(dynamic dynamic)
        {
            var txt = ((string)dynamic)?.NullIfBlank();
            return txt?.Trim();
        }


        public static int ID(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (int.TryParse(str, out int id))
                return id;
            else
                throw Bad.Cast(str, id);
        }


        public static decimal Decimal(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (decimal.TryParse(str, out decimal num))
                return num;
            else
                throw Bad.Cast(str, num);
        }


        public static decimal? Decimal_(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (decimal.TryParse(str, out decimal num))
                return num;
            else
                return null;
        }


        public static decimal DecimalOrZero(dynamic dynamic)
            => As.Decimal_(dynamic) ?? 0M;


        public static bool Bool(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (str.IsBlank()) return false;
            str = str.Trim().ToLower();

            switch (str)
            {
                case "1":
                case "y":
                case "yes":
                case "t":
                case "true": return true;

                case "0":
                case "n":
                case "no":
                case "f":
                case "false": return false;

                default: throw Bad.Cast(str, true);
            }
        }


        public static DateTime Date(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (DateTime.TryParse(str, out DateTime date))
                return date.Date.ToLocalTime();
            else
                throw Bad.Cast(str, date);
        }
    }
}
