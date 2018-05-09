using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.CollectorsRepository;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public static class MarketStateDBFile
    {
        public static MarketStateDB Load(string marketDbFilePath, string currentUser)
        {
            var all          = new MarketStateDB();
            var db           = new SharedLiteDB(marketDbFilePath, currentUser);
            all.Stalls       = new StallsRepo1(new StallsCollection(db), all);
            all.Collectors   = new CollectorsRepo1(new CollectorsCollection(db), all);
            all.Sections     = new SectionsRepo1(new SectionsCollection(db), all);
            all.ActiveLeases = new ActiveLeasesRepo1(new ActiveLeasesCollection(db), all);
            return all;
        }
    }
}
