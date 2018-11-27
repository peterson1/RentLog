using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyColxnsReport : UIList<SectionColxnsRow>
    {
        public DailyColxnsReport(DateTime date, ITenantDBsDir tenantDBsDir)
        {
            Sources = tenantDBsDir;
            Date    = date;
            GenerateFrom(tenantDBsDir);
        }


        public ITenantDBsDir  Sources        { get; }
        public DateTime       Date           { get; }
        public decimal        TotalRent      => this.Sum(_ => _.Rent     ?? 0);
        public decimal        TotalRights    => this.Sum(_ => _.Rights   ?? 0);
        public decimal        TotalElectric  => this.Sum(_ => _.Electric ?? 0);
        public decimal        TotalWater     => this.Sum(_ => _.Water    ?? 0);
        public decimal        TotalAmbulant  => this.Sum(_ => _.Ambulant ?? 0);
        public decimal        SectionsTotal  => TotalRent  + TotalRights + TotalElectric
                                              + TotalWater + TotalAmbulant;

        public Dictionary<int, decimal>  Others  { get; } = new Dictionary<int, decimal>();

        public decimal  CollectionsSum => SectionsTotal + Others.Sum(_ => _.Value);
        public decimal  DepositsSum    { get; private set; }


        private void GenerateFrom(ITenantDBsDir dir)
        {
            if (Date > dir.Collections.LastPostedDate())
                throw Bad.Arg("Daily Colxns Date", $"{Date:d MMM yyyy}");

            FillSectionCollections(dir);
            FillOtherCollections(dir);
            DepositsSum = GetTotalDeposits(dir);

            if (CollectionsSum != DepositsSum)
                throw Bad.Data($"Collections sum ({CollectionsSum:N2}) for [{Date:d-MMM-yyyy}] does not match deposits sum ({DepositsSum:N2}).");

            this.SetSummary(SectionColxnsRow.GetSummary(this));
        }


        private void FillSectionCollections(ITenantDBsDir dir)
        {
            this.Clear();

            foreach (var sec in dir.MarketState.Sections.GetAll())
            {
                var row = new SectionColxnsRow(sec, Date, dir);
                if (row.Total != 0)
                    this.Add(row);
            }

            this.Add(new OfficeColxnsRow(Date, dir));
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
