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
        protected abstract MarketStateDB   GetMarketState ();


        public IDailyBillsRepo GetRepo(LeaseDTO lse) => GetRepo(lse.Id);


        public DailyBillDTO GetBill(LeaseDTO lse, DateTime date)
        {
            var repo = GetRepo(lse);
            var id   = date.ToBillID();
            return repo.Find(id, false);
        }


        public BillAmounts TotalOverdues(DateTime date)
        {
            var list = new List<LeaseBalanceRow>();
            var mkt  = GetMarketState();

            foreach (var lse in mkt.ActiveLeases.GetAll())
                list.Add(GetBalance(lse, date));

            foreach (var lse in mkt.InactiveLeases.GetAll())
                list.Add(GetBalance(lse, null));

            list.RemoveAll(_ => NoOverdue(_, date));
            //var ord = list.OrderByDescending(_ => _.Rights).ToList();

            return new BillAmounts
            {
                Rent     = list.Sum  (_ => _.Rent    .ZeroIfNullOrNegative()),
                Rights   = list.Where(_ => IsRightsOverdue(_, date))
                               .Sum  (_ => _.Rights  .ZeroIfNullOrNegative()),
                Electric = list.Sum  (_ => _.Electric.ZeroIfNullOrNegative()),
                Water    = list.Sum  (_ => _.Water   .ZeroIfNullOrNegative()),
            };
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

            if (!row.IsActive) return true;
            if (date <= row.DTO.RightsDueDate) return true;
            if ((row.Rights ?? 0) <= 0) return true;

            return false;
        }


        private LeaseBalanceRow GetBalance(LeaseDTO lse, DateTime? date)
        {
            var repo = GetRepo(lse);
            var bals = date.HasValue
                     ? repo.Find(date.Value.ToBillID(), false)
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
