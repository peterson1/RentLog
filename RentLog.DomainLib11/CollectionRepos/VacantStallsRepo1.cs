using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class VacantStallsRepo1 : SimpleRepoShimBase<StallDTO>, IVacantStallsRepo
    {
        public VacantStallsRepo1(ISimpleRepo<StallDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }


    public static class VacantStallsDictionaryExtensions
    {
        public static void UpdateAllLists(this Dictionary<int, IVacantStallsRepo> dict, MarketStateDB marketState)
        {
            foreach (var keyVal in dict)
            {
                var repo = keyVal.Value;
                var recs = GetVacantsFor(keyVal.Key, marketState);

                repo.Drop();
                repo.Insert(recs, true);
            }
        }


        private static IEnumerable<StallDTO> GetVacantsFor(int secID, MarketStateDB mkt)
        {
            var occupieds = mkt.ActiveLeases.GetAll()
                               .Where  (_ => _.Stall.Section.Id == secID)
                               .Select (_ => _.Stall.Id)
                               .ToList ();

            return mkt.Stalls.ForSection(secID)
                             .Where     (_ => !occupieds.Contains(_.Id))
                             .ToList    ();
        }
    }
}
