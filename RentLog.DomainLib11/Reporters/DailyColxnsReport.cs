using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyColxnsReport : Dictionary<int, SectionColxnsRow>
    {
        private DailyColxnsReport(DateTime date)
        {
            Date = date;
        }


        public DateTime Date { get; }


        private void Generate(ITenantDBsDir dir)
        {
            this.Clear();

            foreach (var sec in dir.MarketState.Sections.GetAll())
                this.Add(sec.Id, new SectionColxnsRow(sec, Date, dir));

            this.Add(0, new OfficeColxnsRow(Date, dir));
        }


        public static DailyColxnsReport Load(ITenantDBsDir tenantDBsDir, DateTime date)
        {
            var rep = new DailyColxnsReport(date);
            rep.Generate(tenantDBsDir);
            return rep;
        }
    }
}
