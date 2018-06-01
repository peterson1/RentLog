using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib45;

namespace RentLog.Tests.SampleDBs
{
    internal class SampleArgs : AppArguments
    {
        internal static string DirName;


        protected override MarketStateDB GetMarketStateDB()
        {
            var dbPath = SampleDir.FindDB(DirName);
            return new MarketStateDBFile(dbPath, "Test Runner");
        }


        internal static SampleArgs HelenAblen_Dry8()
        {
            SampleArgs.DirName = SampleDir.HELEN_ABLEN;
            return new SampleArgs();
        }
    }
}
