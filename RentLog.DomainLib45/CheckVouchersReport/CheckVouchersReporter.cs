using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.DataSources;
using System;

namespace RentLog.DomainLib45.CheckVouchersReport
{
    public class CheckVouchersReporter
    {

        public static void Launch(ITenantDBsDir dir)
        {
            var now = DateTime.Now.Date;
            var bgn = new DateTime(now.Year, now.Month - 1, 1);
            var end = bgn.AddMonths(1).AddDays(-1);

            if (!PopUpInput.TryGetDateRange("Clearing Dates of Checks", out (DateTime Start, DateTime End) rng, bgn, end)) return;


        }
    }
}
