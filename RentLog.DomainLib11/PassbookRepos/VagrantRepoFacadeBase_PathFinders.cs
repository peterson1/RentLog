using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.DomainLib11.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.PassbookRepos
{
    public abstract partial class VagrantRepoFacadeBase
    {
        protected abstract ISimpleRepo<PassbookRowDTO> ConnectToDB(string databasePath);
        protected abstract string GetDatabasePath(DateTime date);


        public List<PassbookRowDTO> RowsFor(DateTime date)
            => FindRepo(date).Find(_ 
                => _.DateOffset == date.DaysSinceMin());


        private ISimpleRepo<PassbookRowDTO> FindRepo(DateTime date)
            => ConnectToDB(GetDatabasePath(date));


        public List<PassbookRowDTO> RowsFor(DateTime startDate, DateTime endDate)
        {
            var min   = startDate.DaysSinceMin();
            var max   = endDate.DaysSinceMin();
            var rows  = new List<PassbookRowDTO>();

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
    }
}
