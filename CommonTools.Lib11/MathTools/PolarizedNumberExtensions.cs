using System;

namespace CommonTools.Lib11.MathTools
{
    public static class PolarizedNumberExtensions
    {
        public static decimal ZeroIfNegative(this decimal num) 
            => Math.Max(0, num);

        public static decimal ZeroIfNullOrNegative(this decimal? num)
            => Math.Max(0, num ?? 0);
    }
}
