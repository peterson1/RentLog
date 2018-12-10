using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForActiveLeases
    {
        public static void NoRoundOff(ITenantDBsDir dir)
        {
            var repo = dir.MarketState.ActiveLeases;
            var lses = repo.GetAll();

            foreach (var lse in lses)
                lse.Rent.PenaltyRule = RentPenalty.DailySurcharge_NoRoundOff;

            repo.DropAndInsert(lses, true, true);
        }


        public static void NonExpiringLeasesForSome(ITenantDBsDir dir)
        {
            var repo = dir.MarketState.ActiveLeases;
            var lses = repo.GetAll();

            foreach (var lse in lses)
            {
                if (lse.Rights.TotalAmount == 7_000)
                    lse.ContractEnd = DateTime.MaxValue;
            }

            repo.DropAndInsert(lses, true, true);
        }


        public static void SetContractYears(int yearCount, ITenantDBsDir dir)
        {
            var repo = dir.MarketState.ActiveLeases;
            var lses = repo.GetAll();

            foreach (var lse in lses)
                lse.ContractEnd = lse.ContractStart.AddYears(yearCount);

            repo.DropAndInsert(lses, true, true);
        }


        public static void ResetStart(DateTime startDate, ITenantDBsDir dir)
        {
            var repo = dir.MarketState.ActiveLeases;
            var lses = repo.GetAll();

            foreach (var lse in lses)
            {
                lse.ContractStart        = startDate;
                lse.ContractEnd          = GetContractEnd(lse);
                lse.Rent.GracePeriodDays = 0;
            }

            repo.DropAndInsert(lses, true, true);
        }


        private static DateTime? GetContractEnd(LeaseDTO lse)
        {
            if (!lse.ContractEnd.HasValue)
                return (DateTime?)null;

            else if (lse.ContractEnd.Value.Year == 9999)
                return (DateTime?)null;

            else
                return lse.ContractStart.AddYears(1);
        }


        public static Action RebuildSoA(ITenantDBsDir dir, 
            out string jobDesc, out bool canRun)
        {
            canRun  = true;
            jobDesc = "Rebuild SoA for all Active Leases";
            return () =>
            {
                var lses = dir.MarketState.ActiveLeases.GetAll();

                foreach (var lse in lses)
                    dir.Balances.GetRepo(lse).RecomputeAll();
            };
        }
    }
}
