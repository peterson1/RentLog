using CommonTools.Lib11.GoogleTools;
using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib45;

namespace RentLog.Tests.SampleDBs
{
    internal class SampleArgs : AppArguments
    {
        internal static string DirName;

        public SampleArgs()
        {
            IsValidUser = true;
            Credentials = new FirebaseCredentials();
            Credentials.Roles = "Supervisor";
        }


        protected override MarketStateDB GetMarketStateDB()
        {
            var dbPath = SampleDir.FindDB(DirName);
            return new MarketStateDBFile(dbPath, "Test Runner");
        }


        internal static SampleArgs Lease197()
        {
            SampleArgs.DirName = SampleDir.LEASE_197;
            return new SampleArgs();
        }
    }
}
