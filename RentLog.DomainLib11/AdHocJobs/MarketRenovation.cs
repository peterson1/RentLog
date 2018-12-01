using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class MarketRenovation
    {
        public static void DecommissionOldStalls
            (ITenantDBsDir dir, int oldSectionsMaxId, DateTime terminationDate)
        {
            TerminateOldLeases  (dir, oldSectionsMaxId, terminationDate);
            ZeroOutRentBalances (dir, terminationDate);
            MarkOldStallsAsNonOperational(dir, oldSectionsMaxId);
            MarkNewStallsAsOperational(dir, oldSectionsMaxId);
        }


        public static void TerminateOldLeases(ITenantDBsDir dir, int oldSectionsMaxId, DateTime terminationDate)
        {
            var mkt     = dir.MarketState;
            var actives = mkt.ActiveLeases.GetAll();

            foreach (var lse in actives)
            {
                if (lse.Stall.Section.Id <= oldSectionsMaxId)
                    mkt.DeactivateLease(lse, "Market Renovation", terminationDate);
            }
        }


        private static void ZeroOutRentBalances(ITenantDBsDir dir, DateTime terminationDate)
        {
            var adjs = (dir.Collections.For      (terminationDate)
                     ?? dir.Collections.CreateFor(terminationDate))
                    .BalanceAdjs;

            var inactvs = dir.MarketState.InactiveLeases.GetAll();

            foreach (var lse in inactvs)
            {
                var bill   = dir.Balances.GetBill(lse, terminationDate);
                var endBal = bill?.For(BillCode.Rent)?.ClosingBalance ?? 0;
                if (endBal == 0) continue;

                adjs.Insert(new BalanceAdjustmentDTO
                {
                    DocumentRef  = "[system-auto]",
                    Reason       = "Market Renovation",
                    LeaseId      = lse.Id,
                    BillCode     = BillCode.Rent,
                    AmountOffset = endBal * -1,
                });
            }
        }


        public static void MarkOldStallsAsNonOperational(ITenantDBsDir dir, int oldSectionsMaxId)
        {
            var repo      = dir.MarketState.Stalls;
            var allStalls = repo.GetAll();

            foreach (var stall in allStalls)
            {
                if (stall.Section.Id <= oldSectionsMaxId)
                    stall.IsOperational = false;
            }
            repo.DropAndInsert(allStalls, true, false);
        }


        public static void MarkNewStallsAsOperational(ITenantDBsDir dir, int oldSectionsMaxId)
        {
            var repo      = dir.MarketState.Stalls;
            var allStalls = repo.GetAll();

            foreach (var stall in allStalls)
            {
                if (stall.Section.Id > oldSectionsMaxId)
                    stall.IsOperational = true;
            }
            repo.DropAndInsert(allStalls, true, false);
        }
    }
}
