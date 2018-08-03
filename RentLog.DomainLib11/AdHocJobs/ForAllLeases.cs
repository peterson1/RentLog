using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForAllLeases
    {
        public static void NoGraceThenRebuild_serial(DateTime minDate, ITenantDBsDir dir)
        {
            var mkt = dir.MarketState;

            foreach (var lse in mkt.ActiveLeases.GetAll())
            {
                lse.Rent.GracePeriodDays = 0;
                mkt.ActiveLeases.Update(lse);
                RebuildSoaFor(lse, minDate, dir);
            }

            foreach (var lse in mkt.InactiveLeases.GetAll())
            {
                lse.Rent.GracePeriodDays = 0;
                mkt.InactiveLeases.Update(lse);
                RebuildSoaFor(lse, minDate, dir);
            }
        }


        //public static List<Action> NoGraceThenRebuild(DateTime minDate, ITenantDBsDir dir)
        //{
        //    var mkt = dir.MarketState;
        //    var jobs = new List<Action>();

        //    foreach (var lse in mkt.ActiveLeases.GetAll())
        //        jobs.Add(() =>
        //        {
        //            if (lse.Rent.GracePeriodDays != 0)
        //            {
        //                lse.Rent.GracePeriodDays = 0;
        //                mkt.ActiveLeases.Update(lse);
        //                RebuildSoaFor(lse, minDate, dir);
        //            }
        //        });

        //    foreach (var lse in mkt.InactiveLeases.GetAll())
        //        jobs.Add(() =>
        //        {
        //            if (lse.Rent.GracePeriodDays != 0)
        //            {
        //                lse.Rent.GracePeriodDays = 0;
        //                mkt.InactiveLeases.Update(lse);
        //                RebuildSoaFor(lse, minDate, dir);
        //            }
        //        });

        //    return jobs;
        //}


        public static List<Action> RebuildSoaFrom(DateTime minDate, ITenantDBsDir dir)
        {
            var mkt  = dir.MarketState;
            var jobs = new List<Action>();

            foreach (var lse in mkt.ActiveLeases.GetAll())
                jobs.Add(() => RebuildSoaFor(lse, minDate, dir));

            foreach (var lse in mkt.InactiveLeases.GetAll())
                jobs.Add(() => RebuildSoaFor(lse, minDate, dir));

            return jobs;
        }


        private static void RebuildSoaFor(LeaseDTO lse, DateTime minDate, ITenantDBsDir dir)
        {
            var repo = dir.Balances.GetRepo(lse);
            DeleteRowsEarlierThan(minDate, repo);
            repo.UpdateFrom(minDate);
        }


        private static void DeleteRowsEarlierThan(DateTime minDate, IDailyBillsRepo repo)
        {
            var minId = minDate.DaysSinceMin();
            var rows  = repo.Find(_ => _.Id < minId);
            repo.Delete(rows);
        }
    }
}
