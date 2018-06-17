using CommonTools.Lib11.GoogleTools;
using FluentAssertions;
using RentLog.DatabaseLib.DatabaseFinders;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib45;
using System;
using System.IO;

namespace RentLog.Tests.SampleDBs
{
    internal class SampleArgs : AppArguments
    {
        internal static string DirName;

        public SampleArgs()
        {
            IsValidUser       = true;
            Credentials       = new FirebaseCredentials();
            Credentials.Roles = "Supervisor";
            CurrentBankAcct   = new BankAccountDTO { Id = 1 };
        }


        protected override MarketStateDB GetMarketStateDB()
        {
            var dbPath = FindDB(DirName);
            return new MarketStateDBFile(dbPath, "Test Runner");
        }


        private static string FindDB(string folderName)
        {
            var dbPath = Path.Combine(@"..\..\SampleDBs",
                            folderName, "MarketState.ldb");
            File.Exists(dbPath).Should().BeTrue();
            return dbPath;
        }
    }
}
