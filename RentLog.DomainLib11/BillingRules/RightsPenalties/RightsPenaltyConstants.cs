using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RentLog.DomainLib11.BillingRules.RightsPenalties
{
    public class RightsPenalty
    {
        public const string DailyAfter90Days = "Daily Surcharge after 90 days";

        public static ReadOnlyCollection<string> Rules = new ReadOnlyCollection<string>(new List<string>
        {
            DailyAfter90Days
        });
    }
}
