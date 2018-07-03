using CommonTools.Lib11.GoogleTools;
using FluentAssertions;
using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib45;
using System.IO;
using System.Threading;

namespace RentLog.Tests.SampleDBs
{
    public class SampleArgs : AppArguments
    {
        internal static string DirPath = @"..\..\SampleDBs";
        internal static string DirName;

        public SampleArgs(string roles)
        {
            IsValidUser       = true;
            Credentials       = new FirebaseCredentials();
            Credentials.Roles = roles;
            CurrentBankAcct   = new BankAccountDTO { Id = 1 };
        }


        protected override MarketStateDB GetMarketStateDB()
        {
            var dbPath = FindDB(DirName);
            return new MarketStateDBFile(dbPath, "Test Runner");
        }


        public static string FindDB(string folderName)
        {
            var dbPath = Path.Combine(DirPath,
                            folderName, "MarketState.ldb");

            while (!File.Exists(dbPath))
                Thread.Sleep(100);

            File.Exists(dbPath).Should().BeTrue();
            return dbPath;
        }
    }
}
