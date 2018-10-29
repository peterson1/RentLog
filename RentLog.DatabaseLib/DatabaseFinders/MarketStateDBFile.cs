﻿using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.BankAccountsRepository;
using RentLog.DatabaseLib.CollectorsRepository;
using RentLog.DatabaseLib.GLAccountsRepository;
using RentLog.DatabaseLib.LeasesRepository;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.IO;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class MarketStateDBFile : MarketStateDB
    {
        private const string SYSTEM_NAME_KEY = "SystemName";
        private const string BRANCH_NAME_KEY = "BranchName";
        private string _mktDbPath;
        private string _currUsr;


        public MarketStateDBFile(string marketDbFilePath, string currentUser)
        {
            _mktDbPath     = marketDbFilePath;
            _currUsr       = currentUser;
            var mktDb      = new SharedLiteDB(_mktDbPath, _currUsr);
                               
            DatabasePath   = marketDbFilePath;
            CurrentUser    = currentUser;
            //SystemName     = mktDb.Metadata[SYSTEM_NAME_KEY];
            //BranchName     = mktDb.Metadata[BRANCH_NAME_KEY];
                           
            Stalls         = new StallsRepo1(new StallsCollection(mktDb), this);
            Collectors     = new CollectorsRepo1(new CollectorsCollection(mktDb), this);
            BankAccounts   = new BankAccountsRepo1(new BankAccountsCollection(mktDb), this);
            Sections       = new SectionsRepo1(new SectionsCollection(mktDb), this);
            ActiveLeases   = new ActiveLeasesRepo1(new ActiveLeasesCollection(mktDb), this);
            InactiveLeases = new InactiveLeasesRepo1(new InactiveLeasesCollection(mktDb), this);
        }


        public override IGLAccountsRepo GLAccounts
        {
            get => base.GLAccounts ?? TryLoadPassbookDB(_ => _.GLAccounts);
            set => base.GLAccounts = value;
        }


        private SharedLiteDB MktDB => new SharedLiteDB(_mktDbPath, _currUsr);

        public override string SystemName
        {
            get => MktDB.Metadata[SYSTEM_NAME_KEY];
            set => MktDB.Metadata[SYSTEM_NAME_KEY] = value;
        }

        public override string BranchName
        {
            get => MktDB.Metadata[BRANCH_NAME_KEY];
            set => MktDB.Metadata[BRANCH_NAME_KEY] = value;
        }


        private T TryLoadPassbookDB<T>(Func<MarketStateDB, T> getter)
        {
            //var dir    = Path.GetDirectoryName(_mktDbPath);
            //var dbPath = Path.Combine(dir, PassbookDBFile.FILENAME);
            //
            //if (!File.Exists(dbPath))
            //    throw Missing.File(dbPath, "Passbook DB file");

            var dbPath = PassbookDBFile.GetDbPath(this);
            var pbkDb  = new SharedLiteDB(dbPath, _currUsr);
            GLAccounts = new GLAccountsRepo1(new GLAccountsCollection(pbkDb), this);
            return getter(this);
        }
    }
}
