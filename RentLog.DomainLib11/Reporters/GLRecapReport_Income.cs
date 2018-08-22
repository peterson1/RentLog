using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public static class GLRecapReport_Income
    {
        public static void LoadIncomeSources(this GLRecapReport rep)
        {
            var subRep = rep.Single(_ => _.AccountType == GLAcctType.Income);
            foreach (var date in rep.StartDate.EachDayUpTo(rep.EndDate))
                AddIncomeRows(date, subRep, rep.DBsDir);
        }


        private static void AddIncomeRows(DateTime date, GLRecapCategory incomeRep, ITenantDBsDir dir)
        {

            //throw new NotImplementedException();
        }
    }
}
