using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RentLog.DomainLib11.BillingRules.RentPenalties
{
    public class RentPenalty
    {
        public const string DailySurcharge            = "Daily Surcharge";
        public const string DailySurcharge_NoRoundOff = "Daily Surcharge (No Round-off)";
        public const string MonthlySurcharge          = "Monthly Surcharge";

        public static ReadOnlyCollection<string> Rules = new ReadOnlyCollection<string>(new List<string>
        {
            DailySurcharge,
            DailySurcharge_NoRoundOff,
            MonthlySurcharge,
        });
    }
}
