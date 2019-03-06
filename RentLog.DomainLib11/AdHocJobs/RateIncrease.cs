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
                        2950145, // Dizon, Jhoana   DRY 018
                        2978980, // Cuevas, Chi Chi DRY 023
                        2919287, // Caragos, Ma. Concepcion DRY 023- A
                        2933259, // Panopio, Aldrich    DRY 028
                        2873830, // Salazar, Jenevive Ann   DRY 031
                        2907663, // Robles, Marissa DRY 056
                        2979007, // Sarip, Sonaya   DRY 058
                        2935600, // Delos Santos, Urbana    DRY 063
                        2979004, // H. Carem, Moktar    DRY 085
                        2979005, // H. Carem, Moktar    DRY 086
                        2979011, // Tadman, Aiden   DRY 227
                        2601253, // Hernandez, John Michael DRY 228
                        2970561, // Banggolay, Catherine    DRY 230
                        2767682, // Agustin, Antonio    DRY 232
                        2727202, // Limbona, Haniyah    DRY 237
                        2847542, // Manalo, Teresita    DRY 238
                        2978982, // Macalabo, Mokindi   DT 002
                        2979006, // Santiago, Evangeline    DT 027
                        2979075, // Imam, Soraida   DT 038
                        2978983, // Saumay, Alladin DT 040
                        2943102, // Gonzaga, Melanio Jr WET 023
                        2943103, // Gonzaga, Melanio Jr WET 024
                        2978992, // Santiago, Alicia    WET 115
                        2978999, // Manahan, Jocelyn    WET 116
                        2679454, // Gemodiano, Josefino WET 123
                        2679420, // Gemodiano, Josefino WET 124
                        2870341, // Castillo, Elsa  WET 149
                        2956954, // Castillo, Elsa  WET 150
                        2782255, // Solangsawa, Rosalie WET 185
                        2782270, // Solangsawa, Rosalie WET 186
                        2870234, // Abejar, Jonathan    WET 190
                        2979029, // Joaquin, Jed    WET 194
                        2897367, // Castro, Teresita    WT 005
                        2899941, // Castro, Teresita    WT 006
                        2958846, // Pacheco, Rochelle   WT 008
                        2934029, // Sevilla, Florinda   WT 015
                        2721579, // Gomez, Blanca   WT 016
                        2979043, // Garcia, Donato  WT 018
                        2665163, // Garcia, Donato  WT 019
                        2349949, // Berlon, Crizia  WT 039
                        2347210, // Berlon, Crizia  WT 040
                        2850674, // Magbitang, Ruby WT 069
                        2791525, // Palma, April Lyn    WT 128
                        2817651, // Silva, Asuncion WT 129
                        2816153, // Tenorio, Eliza Marie    WT 130
                        2816154, // Tenorio, Eliza Marie    WT 131
                        2979019, // Fernandez, Ruby Ann WT 140
                        2975689, // De Asis, Reglend    WT 141
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
