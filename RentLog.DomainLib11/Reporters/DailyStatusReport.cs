using RentLog.DomainLib11.DataSources;
using System;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyStatusReport
    {
        public DailyStatusReport(DateTime date, ITenantDBsDir tenantDBsDir)
        {
            Date         = date;
            ColxnsReport = new DailyColxnsReport(date, tenantDBsDir);
            GenerateFrom(tenantDBsDir);
        }


        public DateTime           Date          { get; }
        public DailyColxnsReport  ColxnsReport  { get; }


        private void GenerateFrom(ITenantDBsDir tenantDBsDir)
        {
            throw new NotImplementedException();
        }
    }
}
