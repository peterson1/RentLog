using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.Reporters
{
    public class CollectorsPerformanceReport : Dictionary<int, CollectorPerformanceRow>
    {
        public CollectorsPerformanceReport(DateTime date, ITenantDBsDir tenantDBsDir)
        {

        }
    }
}
