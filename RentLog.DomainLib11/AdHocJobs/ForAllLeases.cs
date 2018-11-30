using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.AdHocJobs
{
    public partial class ForAllLeases
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
            repo.RecomputeFrom(minDate);
        }


        private static void DeleteRowsEarlierThan(DateTime minDate, IDailyBillsRepo repo)
        {
            var minId = minDate.DaysSinceMin();
            var rows  = repo.Find(_ => _.Id < minId);
            repo.Delete(rows);
        }


        //public static void SetSectionID(ITenantDBsDir dir)
        //{
        //    var activs = dir.MarketState.ActiveLeases.GetAll();
        //    foreach (var lse in activs)
        //        lse.SectionID = lse.Stall.Section.Id;
        //    dir.MarketState.ActiveLeases.DropAndInsert(activs, true, false);

        //    var inactvs = dir.MarketState.InactiveLeases.GetAll();
        //    foreach (var lse in inactvs)
        //        lse.SectionID = lse.Stall.Section.Id;
        //    dir.MarketState.InactiveLeases.DropAndInsert(inactvs, true, false);
        //}
    }
}
