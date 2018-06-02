﻿using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class ColxnSummaryReport : List<DailyColxnsReport>
    {
        private const string LONG_FMT = "MMMM d, yyyy";


        public ColxnSummaryReport(DateTime startDate, DateTime endDate, ITenantDBsDir tenantDBsDir)
        {
            this.StartDate = startDate;
            this.EndDate   = endDate;
            GenerateFrom(tenantDBsDir);
        }

        public DateTime  StartDate  { get; }
        public DateTime  EndDate    { get; }
        public string    Title      { get; set; } = "Collection Summary";

        public Dictionary<int, CollectionAmounts> SectionTotals { get; } = new Dictionary<int, CollectionAmounts>();
        public Dictionary<int, decimal>           OtherTotals   { get; } = new Dictionary<int, decimal>();
        public Dictionary<int, string>            GLNames       { get; } = new Dictionary<int, string>();

        public string   DateRangeText    => $"{StartDate.ToString(LONG_FMT)} to {EndDate.ToString(LONG_FMT)}";
        public decimal  TotalCollections => this.Sum(_ => _.CollectionsSum);
        public decimal  TotalDeposits    => this.Sum(_ => _.DepositsSum);


        private void GenerateFrom(ITenantDBsDir dir)
        {
            FillMainList(dir);
            FillLookups(dir);
            FillSectionTotals(dir);
            FillOtherTotals(dir);
        }


        private void FillMainList(ITenantDBsDir dir)
        {
            this.Clear();

            foreach (var date in StartDate.EachDayUpTo(EndDate))
                this.Add(new DailyColxnsReport(date, dir));
        }


        private void FillSectionTotals(ITenantDBsDir dir)
        {
            SectionTotals.Clear();

            foreach (var sec in dir.MarketState.Sections.GetAll())
                SectionTotals.Add(sec.Id, GetCollectionAmounts(sec.Id));

            SectionTotals.Add(0, GetCollectionAmounts(0));
        }


        private CollectionAmounts GetCollectionAmounts(int sectionId)
            => new CollectionAmounts
            {
                Rent     = this.Sum(_ => _[sectionId].Rent    ),
                Rights   = this.Sum(_ => _[sectionId].Rights  ),
                Electric = this.Sum(_ => _[sectionId].Electric),
                Water    = this.Sum(_ => _[sectionId].Water   ),
                Ambulant = this.Sum(_ => _[sectionId].Ambulant),
            };


        private void FillLookups(ITenantDBsDir dir)
        {
            GLNames.Clear();
            foreach (var gl in dir.MarketState.GLAccounts.GetAll())
                GLNames.Add(gl.Id, gl.Name);
        }


        private void FillOtherTotals(ITenantDBsDir dir)
        {
            OtherTotals.Clear();

            var glAcctIDs = this.SelectMany(_ => _.Others.Keys)
                                .GroupBy   (_ => _)
                                .Select    (_ => _.First());
            foreach (var id in glAcctIDs)
            {
                var sum = 0M;
                foreach (var row in this)
                {
                    if (row.Others.TryGetValue(id, out decimal amt))
                        sum += amt;
                }
                OtherTotals.Add(id, sum);
            }
        }
    }
}