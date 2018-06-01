using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.Reporters
{
    public class ColxnSummaryReport : List<DailyColxnsReport>
    {
        public ColxnSummaryReport(DateTime startDate, DateTime endDate, ITenantDBsDir tenantDBsDir)
        {

        }
    }
}
