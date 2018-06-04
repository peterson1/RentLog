using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.StateTransitions
{
    public class MarketDayCloser
    {
        public static List<Action> GetActions(ITenantDBsDir dir)
        {
            var actions = new List<Action>();
            var unclosd = dir.Collections.UnclosedDate();
            var activs  = dir.MarketState.ActiveLeases.GetAll();

            foreach (var lse in activs)
                actions.Add(() => dir.Balances.GetRepo(lse)
                                    .OpenNextDay(unclosd));

            actions.Add(() 
                => dir.Collections.For(unclosd.AddDays(1), true));

            actions.Add(()
                => dir.Collections.For(unclosd, false).MarkAsPosted());

            return actions;
        }
    }
}
