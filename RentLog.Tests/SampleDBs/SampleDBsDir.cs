using FluentAssertions;
using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System.IO;

namespace RentLog.Tests.SampleDBs
{
    internal class SampleDBsDir
    {
        public SampleDBsDir(string marketStateDbPath, string currentUser)
        {
            MarketState = new MarketStateDBFile  (marketStateDbPath, currentUser);
            Balances    = new BalancesLocalDir   (MarketState);
            Collections = new CollectionsLocalDir(MarketState);
        }


        public MarketStateDB     MarketState  { get; }
        public ICollectionsDir   Collections  { get; }
        public IBalanceDB        Balances     { get; }
    }


    internal static class SampleDir
    {
        internal const string HELEN_ABLEN  = "2018-05-12 Helen Ablen - 197 - DRY 008";
        internal const string MAY_19_GARAY = "2018-05-19 F_Garay";


        internal static SampleDBsDir HelenAblen_Dry8(out LeaseDTO lse)
            => Load(HELEN_ABLEN, 197, out lse);


        internal static SampleDBsDir May_19_F_Garay() => Load(MAY_19_GARAY);


        internal static string FindDB(string folderName)
        {
            var dbPath = Path.Combine(@"..\..\SampleDBs",
                            folderName, "MarketState.ldb");
            File.Exists(dbPath).Should().BeTrue();
            return dbPath;
        }


        internal static SampleDBsDir Load(string folderName) 
            => new SampleDBsDir(FindDB(folderName), "Test Runner");


        internal static SampleDBsDir Load(string folderName, int lseId, out LeaseDTO lse)
        {
            var dir = Load(folderName);
            lse = dir.MarketState.ActiveLeases.Find(lseId, true);
            return dir;
        }
    }
}
