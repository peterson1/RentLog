using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.Reporters
{
    public class StallsInventoryReport : Dictionary<int, StallsInventoryRow>
    {
        public StallsInventoryReport(DateTime date, ITenantDBsDir tenantDBsDir)
        {
            Date = date;
            GenerateFrom(tenantDBsDir);
        }


        public DateTime   Date   { get; }


        private void GenerateFrom(ITenantDBsDir tenantDBsDir)
        {
            var colxnsDB = tenantDBsDir.Collections.For(Date);
            var sections = tenantDBsDir.MarketState.Sections.GetAll();

            foreach (var sec in sections)
                this.Add(sec.Id, new StallsInventoryRow(sec, colxnsDB));
        }
    }
}
