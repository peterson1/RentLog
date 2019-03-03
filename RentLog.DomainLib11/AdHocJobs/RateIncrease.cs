using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class RateIncrease
    {
        private static void ExecuteRateIncrease(LeaseDTO origLse, ITenantDBsDir dir)
        {
            var mkt     = dir.MarketState;
            var lastDte = 28.Feb(2019);
            var reason  = "2019 Rate Increase";
            var inactv  = mkt.DeactivateLease(origLse, reason, lastDte);
            var newActv = CreateNewActive(inactv, lastDte);

            mkt.ActiveLeases.Insert(newActv);
            inactv.ForwardBalancesTo(newActv, dir);
        }


        private static LeaseDTO CreateNewActive(InactiveLeaseDTO old, DateTime lastDte)
        {
            var lse = new LeaseDTO
            {
                Tenant        = old.Tenant,
                Stall         = old.Stall,
                ContractStart = lastDte.AddDays(1).Date,
                ContractEnd   = old.ContractEnd,
                Rent          = old.Rent,
                Rights        = old.Rights,
                RenewedFromID = old.Id,
                ProductToSell = old.ProductToSell,
                Remarks       = old.Remarks,
            };
            lse.Rent.RegularRate     = GetNewRentRate(old);
            lse.Rent.GracePeriodDays = 0;
            return lse;
        }


        private static decimal GetNewRentRate(InactiveLeaseDTO old)
        {
            var newRate  = old.Rent.RegularRate * 1.10M;
            var rounding = MidpointRounding.AwayFromZero;
            return Math.Round(newRate, 0, rounding);
        }


        public static Action ApplyTo(string branchName, ITenantDBsDir dir, out string desc, out bool canRun)
        {
            canRun    = dir.CanRunAdHocTask(false);
            desc      = $"Apply Rate increase for {branchName}";
            return () =>
            {
                var activs = dir.MarketState.ActiveLeases.GetAll();
                var filtrd = RemoveExemptions(activs, branchName);
                foreach (var lse in filtrd)
                    ExecuteRateIncrease(lse, dir);
            };
        }


        private static List<LeaseDTO> RemoveExemptions(List<LeaseDTO> activs, string branchName)
        {
            var exemptions = GetExemptedIDs(branchName);
            return activs.Where(_ => !exemptions.Contains(_.Id)).ToList();
        }


        private static List<int> GetExemptedIDs(string branchName)
        {
            switch (branchName.Trim().ToUpper())
            {
                case "BALAGTAS":
                    return new List<int>
                    {
                    };

                case "MARILAO":
                    return new List<int>
                    {
                        4701985, // Tan, Solen  B 182 - 242
                        4701866, // Vistan, Othessa Lara    A 004    352.00
                        4702068, // Sotto, Mary Joy A 010    352.00
                        4702055, // Mamaril, Manilyn    A 018    352.00
                        4629626, // Cloma, Rosendo  A 021    352.00
                        4702046, // Catana, Cristilyn   A 022    352.00
                        4605077, // Tezado, Erwin   B 091    226.00
                        4702084, // Cornel, Andi    B 101    169.00
                        4702043, // Bagul, Rainida  B 105    211.00
                        4702088, // Ogayon, Luisa   B 179    150.00
                        4702085, // Ogayon, Luisa   B 180    150.00
                        4701868, // Mateo, Henry    B 185    242.00
                        4702032, // Appari, Hazel   DRY 075B     292.00
                        4702036, // Garalde, Roselyn    WET 006  171.00
                        4612810, // Garobo, Lloyd   WET 019  171.00
                        4619259, // Garobo, Lloyd   WET 020  171.00
                        4702089, // Tolentino, Christine    WET 068  226.00
                        4702057, // Emperado, Liezle    WET 083  191.00
                        4702058, // Emperado, Liezle    WET 084  227.00
                        4702059, // Emperado, Liezle    WET 187  42.00
                        4482600, // Luna A005
                        4587943, // Volfango A019
                        4701989, // Rosadiño B181
                    };

                default: return null;
            }
        }
    }
}
