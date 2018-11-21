using CommonTools.Lib11.DateTimeTools;
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


        public static int? ID_(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (int.TryParse(str, out int id))
                return id;
            else
                return null;
        }
        public static int ID(dynamic dynamic)
            => (int?)ID_(dynamic) 
            ?? throw Bad.Cast<int>((string)dynamic);


        public static decimal? Decimal_(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (decimal.TryParse(str, out decimal num))
                return num;
            else
                return null;
        }
        public static decimal Decimal(dynamic dynamic) 
            => (decimal?)Decimal_(dynamic) 
            ?? throw Bad.Cast<decimal>((string)dynamic);


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

                default: throw Bad.Cast<bool>(str);
            }
        }


        public static DateTime? Date_(dynamic dynamic)
        {
            var str = (string)dynamic;
            if (DateTime.TryParse(str, out DateTime date))
                return date.Date.ToLocalTime().Date;
            else
                return null;
        }


        public static DateTime Date(dynamic dynamic)
        {
            var dte = (DateTime?)Date_(dynamic);
            return dte ?? throw Bad.Cast<DateTime>((string)dynamic);
        }


        public static int DateOffset(dynamic dynamic)
            => ((DateTime)As.Date(dynamic)).DaysSinceMin();
    }
}
