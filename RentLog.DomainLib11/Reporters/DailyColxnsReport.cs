using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyColxnsReport : Dictionary<int, SectionColxnsRow>
    {
        private DailyColxnsReport(DateTime date)
        {
            Date = date;
        }


        public DateTime  Date           { get; }
        public decimal   TotalRent      => this.Sum(_ => _.Value.Rent     ?? 0);
        public decimal   TotalRights    => this.Sum(_ => _.Value.Rights   ?? 0);
        public decimal   TotalElectric  => this.Sum(_ => _.Value.Electric ?? 0);
        public decimal   TotalWater     => this.Sum(_ => _.Value.Water    ?? 0);
        public decimal   TotalAmbulant  => this.Sum(_ => _.Value.Ambulant ?? 0);

        public decimal   SectionsTotal  => TotalRent  + TotalRights + TotalElectric
                                         + TotalWater + TotalAmbulant;


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
