using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.JournalVoucherRepos
{
    public abstract class ShardedJournalsRepoBase : IJournalVouchersRepo
    {
        protected abstract ISimpleRepo<JournalVoucherDTO> ConnectToDB(string databasePath);
        protected abstract string GetDatabasePath(DateTime date);


        public List<JournalVoucherDTO> List(DateTime startDate, DateTime endDate)
        {
            var min  = startDate.DaysSinceMin();
            var max  = endDate.DaysSinceMin();
            var rows = new List<JournalVoucherDTO>();

            foreach (var path in GetUniqueDBPaths(startDate, endDate))
                rows.AddRange(ConnectToDB(path)
                    .Find(_ => _.DateOffset >= min
                            && _.DateOffset <= max));
            return rows;
        }


        private List<string> GetUniqueDBPaths(DateTime startDate, DateTime endDate)
            => startDate.EachDayUpTo(endDate)
                        .Select  (_ => GetDatabasePath(_))
                        .GroupBy (_ => _)
                        .Select  (_ => _.First()).ToList();


        public int  Insert (JournalVoucherDTO r) => GetRepo(r).Insert(r);
        public bool Update (JournalVoucherDTO r) => GetRepo(r).Update(r);
        public bool Delete (JournalVoucherDTO r) => GetRepo(r).Delete(r);

        public bool IsValidForInsert (JournalVoucherDTO r, out string whyInvalid) => GetRepo(r).IsValidForInsert(r, out whyInvalid);
        public bool IsValidForUpdate (JournalVoucherDTO r, out string whyInvalid) => GetRepo(r).IsValidForUpdate(r, out whyInvalid);
        public bool IsValidForDelete (JournalVoucherDTO r, out string whyInvalid) => GetRepo(r).IsValidForDelete(r, out whyInvalid);


        private ISimpleRepo<JournalVoucherDTO> GetRepo(JournalVoucherDTO dto)
            => ConnectToDB(GetDatabasePath(dto.TransactionDate));
    }
}
