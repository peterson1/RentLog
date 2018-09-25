using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.MathTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.BalanceRepos
{
    public abstract class BalanceDBBase : IBalanceDB
    {
        public    abstract IDailyBillsRepo GetRepo        (int leaseID);
        public    abstract IDailyBillsRepo GetRepo        (LeaseDTO lease);
        protected abstract MarketStateDB   GetMarketState ();


        //public IDailyBillsRepo GetRepo(LeaseDTO lse) => GetRepo(lse.Id);


        public DailyBillDTO GetBill(LeaseDTO lse, DateTime date)
        {
            var repo = GetRepo(lse);
            var id   = date.DaysSinceMin();
            return repo.Find(id, false);
        }


        public BillAmounts TotalOverdues(DateTime? asOfDate = null)
        {
            var rows = GetOverdueLeases(out BillAmounts totals, asOfDate);
            return totals;
        }


        public List<LeaseBalanceRow> GetOverdueLeases(out BillAmounts totals, DateTime? asOfDate = null)
            => FilterOverdueLeases(0, out totals, asOfDate);


        public List<LeaseBalanceRow> GetOverdueLeases(out BillAmounts totals, SectionDTO section, DateTime? asOfDate = null)
            => FilterOverdueLeases(section.Id, out totals, asOfDate);


        private List<LeaseBalanceRow> FilterOverdueLeases(int sectionId, out BillAmounts totals, DateTime? asOfDate = null)
        {
            var list = new List<LeaseBalanceRow>();
            var mkt  = GetMarketState();
            var date = asOfDate ?? mkt.Collections.LastPostedDate();

            foreach (var lse in mkt.ActiveLeases.GetAll())
            {
                if (sectionId == 0 || lse.Stall.Section.Id == sectionId)
                    list.Add(GetBalance(lse, date));
            }

            foreach (var lse in mkt.InactiveLeases.GetAll())
            {
                if (sectionId == 0 || lse.Stall.Section.Id == sectionId)
                    list.Add(GetBalance(lse, null));
            }

            list.RemoveAll(_ => NoOverdue(_, date));

            totals = new BillAmounts
            {
                Rent     = list.Sum  (_ => _.Rent    .ZeroIfNullOrNegative()),
                Rights   = list.Where(_ => IsRightsOverdue(_, date))
                               .Sum  (_ => _.Rights  .ZeroIfNullOrNegative()),
                Electric = list.Sum  (_ => _.Electric.ZeroIfNullOrNegative()),
                Water    = list.Sum  (_ => _.Water   .ZeroIfNullOrNegative()),
            };
            return list;
        }


        private bool IsRightsOverdue(LeaseBalanceRow row, DateTime date)
        {
            if ((row.Rights ?? 0) <= 0) return false;
            return date > row.DTO.RightsDueDate;
        }


        private bool NoOverdue(LeaseBalanceRow row, DateTime date)
        {
            if (row == null) return true;
            if ((row.Rent ?? 0) > 0) return false;
            //assume zero backrent from here on

            if (row.IsActive 
                && date <= row.DTO.RightsDueDate) return true;

            if ((row.Rights ?? 0) > 0) return false;
            //assume zero rights balance from here on

            return true;
        }


        private LeaseBalanceRow GetBalance(LeaseDTO lse, DateTime? date)
        {
            var repo = GetRepo(lse);
            var bals = date.HasValue
                     ? repo.Find(date.Value.DaysSinceMin(), false)
                     : repo.Latest();
            if (bals == null) return null;

            //return new LeaseBalanceRow(lse)
            //{
            //    Rent     = bals.For(BillCode.Rent    )?.ClosingBalance ?? 0,
            //    Rights   = bals.For(BillCode.Rights  )?.ClosingBalance ?? 0,
            //    Electric = bals.For(BillCode.Electric)?.ClosingBalance ?? 0,
            //    Water    = bals.For(BillCode.Water   )?.ClosingBalance ?? 0,
            //};
            return new LeaseBalanceRow(lse, bals);
        }
    }
}
