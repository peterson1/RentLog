using System;

namespace CommonTools.Lib11.DateTimeTools
{
    public static class DateTimeExtensions
    {
        public static int DaysSinceMin(this DateTime date)
            => (date.Date - DateTime.MinValue).Days;
    }
}
