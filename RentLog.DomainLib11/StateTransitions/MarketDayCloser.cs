using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public class MarketDayCloser
    {
        public static void Run(ITenantDBsDir dir)
        {
            var unclosd = dir.Collections.UnclosedDate();
            var activs  = dir.MarketState.ActiveLeases.GetAll();
            foreach (var lse in activs)
            {
                var repo = dir.Balances.GetRepo(lse);
                
                //todo: test this: inserts record for next day
                repo.Upsert(BillRowForNextDay(unclosd));

                repo.UpdateFrom(unclosd);
            }

            throw new NotImplementedException();
        }

        private static DailyBillDTO BillRowForNextDay(DateTime unclosd) 
            => new DailyBillDTO { Id = unclosd.AddDays(1).ToBillID() };
    }
}
