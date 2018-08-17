using System;

namespace CommonTools.Lib11.DateTimeTools
{
    public enum Month
    {
        Unknown   = 0,
        January   = 1,
        February  = 2,
        March     = 3,
        April     = 4,
        May       = 5,
        June      = 6,
        July      = 7,
        August    = 8,
        September = 9,
        October   = 10,
        November  = 11,
        December  = 12
    }


    public static class MonthExtensions
    {
        public static DateTime MonthFirstDay(this DateTime date)
            => new DateTime(date.Year, date.Month, 1);

        public static DateTime MonthLastDay(this DateTime date)
            => new DateTime(date.Year, date.Month, 
                DateTime.DaysInMonth(date.Year, date.Month));


        public static DateTime FirstDay(this Month month, int year)
            => new DateTime(year, (int)month, 1);

        public static DateTime LastDay(this Month month, int year)
            => month.FirstDay(year).MonthLastDay();
    }
}
