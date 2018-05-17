﻿using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.FileSystemTools;
using CommonTools.Lib45.LiteDbTools;
using RentLog.DatabaseLib.DailyColxnsRepository;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RentLog.DatabaseLib.DatabaseFinders
{
    public class CollectionsLocalDir : CollectionsDirBase
    {
        private const string COLLECTIONS_DIR = "Collections";
        private const string FILENAME_FMT    = "{0:yyyy-MM-dd}_Solo.ldb";

        private string     _dir;
        //private string     _usr;
        private MarketStateDB _mkt;
        private IBalanceDB _balDB;


        public CollectionsLocalDir(MarketStateDB marketStateDB, IBalanceDB balanceDB)
        {
            _mkt   = marketStateDB;
            _dir   = FindCollectionsDir(_mkt.DatabasePath);
            _balDB = balanceDB;
        }


        private static string FindCollectionsDir(string marketDbFilePath)
        {
            var dbFile = marketDbFilePath.MakeAbsolute();
            var mktDir = Path.GetDirectoryName(dbFile);
            var dayDir = Path.Combine(mktDir, COLLECTIONS_DIR);
            if (!Directory.Exists(dayDir))
                Directory.CreateDirectory(dayDir);
            return dayDir;
        }


        protected override IEnumerable<DateTime> FindAllDates()
            => Directory.EnumerateFiles (_dir, "*.ldb")
                        .Select         (_ => AsDate(_));


        private static DateTime AsDate(string fileName)
        {
            var fName = Path.GetFileNameWithoutExtension(fileName);
            var dateStr = fName.Before("_");

            if (DateTime.TryParse(dateStr, out DateTime date))
                return date;
            else
                throw Fault.BadArg("date prefix", dateStr);
        }


        public override ICollectionsDB For (DateTime date)
        {
            var file               = AsFilePath(date);
            var db                 = new SharedLiteDB(file, _mkt.CurrentUser);
            var colxnsDB           = new CollectionsDB1(db.Metadata, _mkt);
            SetIntendedColxns(colxnsDB.IntendedColxns, db);
            colxnsDB.CashierColxns = new CashierColxnsRepo1(new CashierColxnsCollection(db));
            colxnsDB.BalanceAdjs   = new BalanceAdjsRepo1(date, new BalanceAdjsCollection(db), _balDB, this);
            return colxnsDB;
        }


        private void SetIntendedColxns(Dictionary<int, IIntendedColxnsRepo> dict, SharedLiteDB db)
        {
            foreach (var sec in _mkt.Sections.GetAll())
            {
                var colxn = new IntendedColxnsCollection(sec, db);
                var repo  = new IntendedColxnsRepo1(colxn);
                dict.Add(sec.Id, repo);
            }
        }


        private string AsFilePath(DateTime date)
            => Path.Combine(_dir, string.Format(FILENAME_FMT, date));
    }
}
