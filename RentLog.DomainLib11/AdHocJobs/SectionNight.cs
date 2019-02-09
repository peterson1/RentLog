using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using RentLog.DomainLib11.MarketStateRepos;
using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using CommonTools.Lib11.ExceptionTools;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class SectionNight
    {
        public static Action SetNoSurcharge(ITenantDBsDir dir,
            out string jobDesc, out bool canRun)
        {
            canRun  = dir.CanRunAdHocTask(false);
            jobDesc = "Zero-Surcharge for Night Market leases";
            return () =>
            {
                var mkt   = dir.MarketState;
                var secId = 13062;
                EditStallTemplate(mkt, secId);
                EditAndRecompute(dir, secId, mkt.ActiveLeases);
                EditAndRecompute(dir, secId, mkt.InactiveLeases);
            };
        }


        private static void EditStallTemplate(MarketStateDbBase mkt, int secId)
        {
            var repo = mkt.Sections;
            var sec  = repo.Find(secId, true);

            sec.StallTemplate.DefaultRent
                .PenaltyRule = RentPenalty.ZeroSurcharge;

            if (!repo.Update(sec))
                throw Bad.Data("Update(sec) did NOT return true.");
        }


        private static void EditAndRecompute<T>(ITenantDBsDir dir, int secId, ISimpleRepo<T> repo) where T : LeaseDTO
        {
            foreach (var lse in repo.GetAll())
            {
                if (lse.Stall.Section.Id != secId) continue;
                EditLease(lse, repo);
                dir.Balances.GetRepo(lse)?.RecomputeAll();
            }
        }


        private static void EditLease<T>(T lse, ISimpleRepo<T> repo) where T : LeaseDTO
        {
            lse.Rent.PenaltyRule = RentPenalty.ZeroSurcharge;
            if (!repo.Update(lse))
                throw Bad.Data("Update(lse) did NOT return true.");
        }
    }
}
