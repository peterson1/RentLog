using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyColxnsReport : Dictionary<int, SectionColxnsRow>
    {
        public DailyColxnsReport(DateTime date, ITenantDBsDir tenantDBsDir)
        {
            Date = date;
            GenerateFrom(tenantDBsDir);
        }


        public DateTime  Date           { get; }
        public decimal   TotalRent      => this.Sum(_ => _.Value.Rent     ?? 0);
        public decimal   TotalRights    => this.Sum(_ => _.Value.Rights   ?? 0);
        public decimal   TotalElectric  => this.Sum(_ => _.Value.Electric ?? 0);
        public decimal   TotalWater     => this.Sum(_ => _.Value.Water    ?? 0);
        public decimal   TotalAmbulant  => this.Sum(_ => _.Value.Ambulant ?? 0);
        public decimal   SectionsTotal  => TotalRent  + TotalRights + TotalElectric
                                         + TotalWater + TotalAmbulant;

        public Dictionary<int, decimal>  Others  { get; } = new Dictionary<int, decimal>();
        public Dictionary<int, string>   GLName  { get; } = new Dictionary<int, string>();

        public decimal  TotalCollections => SectionsTotal + Others.Sum(_ => _.Value);
        public decimal  TotalDeposits    { get; private set; }


        private void GenerateFrom(ITenantDBsDir dir)
        {
            FillLookups(dir);
            FillSectionCollections(dir);
            FillOtherCollections(dir);
            TotalDeposits = GetTotalDeposits(dir);
        }


        private void FillLookups(ITenantDBsDir dir)
        {
            GLName.Clear();
            foreach (var gl in dir.MarketState.GLAccounts.GetAll())
                GLName.Add(gl.Id, gl.Name);
        }


        private void FillSectionCollections(ITenantDBsDir dir)
        {
            this.Clear();

            foreach (var sec in dir.MarketState.Sections.GetAll())
                this.Add(sec.Id, new SectionColxnsRow(sec, Date, dir));

            this.Add(0, new OfficeColxnsRow(Date, dir));
        }


        private void FillOtherCollections(ITenantDBsDir dir)
        {
            Others.Clear();

            var othrColxns = dir.Collections.For(Date).OtherColxns
                                .GetAll().GroupBy(_ => _.GetGLId());

            foreach (var othr in othrColxns)
                Others.Add(othr.Key, othr.Sum(_ => _.Amount));
        }


        private decimal GetTotalDeposits(ITenantDBsDir dir)
            => dir.Collections.For(Date).BankDeposits.GetAll().Sum(_ => _.Amount);
    }
}
