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
            MarketState = MarketStateDBFile.Load(marketStateDbPath, currentUser);
            Balances    = new BalancesLocalDir(MarketState);
            Collections = new CollectionsLocalDir(MarketState, Balances);
        }


        public MarketStateDB     MarketState  { get; }
        public ICollectionsDir   Collections  { get; }
        public IBalanceDB        Balances     { get; }
    }


    internal static class SampleDir
    {
        internal static SampleDBsDir HelenAblen_Dry8(out LeaseDTO lse)
            => Load("2018-05-12 Helen Ablen - 197 - DRY 008", 197, out lse);


        internal static SampleDBsDir Load(string folderName, int lseId, out LeaseDTO lse)
        {
            var dbPath = Path.Combine(@"..\..\SampleDBs", 
                            folderName, "MarketState.ldb");
            File.Exists(dbPath).Should().BeTrue();
            var dir = new SampleDBsDir(dbPath, "Test Runner");
            lse = dir.MarketState.ActiveLeases.Find(lseId, true);
            return dir;
        }
    }
}
