using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public static class GLRecapReport_Income
    {
        public static void LoadIncomeSources(this GLRecapReport rep)
        {
            var subRep = rep.Single(_ => _.AccountType == GLAcctType.Income);
            var glDict = rep.DBsDir.MarketState.GLAccounts.ToDictionary();
            foreach (var date in rep.StartDate.EachDayUpTo(rep.EndDate))
            {
                //AddIncomeRows(date, subRep, rep.DBsDir);
                subRep.Add(CreateAcctGrp(date, rep.DBsDir, glDict));
            }
        }


        private static GLRecapAccountGroup CreateAcctGrp(DateTime date, ITenantDBsDir dir, Dictionary<int, GLAccountDTO> glDict)
        {
            var dlyColxn  = new DailyColxnsReport(date, dir);
            var colxnAcct = GLAccountDTO.Named($"Daily Collections for {dlyColxn.Date:d-MMM-yyyy}");
            var acctGrp   = new GLRecapAccountGroup(colxnAcct, new List<GLRecapAllocation>
            {
                CreateAllocation(dlyColxn.TotalRent    , "Rent Collections"    , colxnAcct, date),
                CreateAllocation(dlyColxn.TotalRights  , "Rights Collections"  , colxnAcct, date),
                CreateAllocation(dlyColxn.TotalElectric, "Electric Collections", colxnAcct, date),
                CreateAllocation(dlyColxn.TotalWater   , "Water Collections"   , colxnAcct, date),
                CreateAllocation(dlyColxn.TotalAmbulant, "Ambulant Collections", colxnAcct, date),
            });

            foreach (var othr in dlyColxn.Others)
            {
                var glAcct = glDict[othr.Key];
                acctGrp.Add(CreateAllocation(othr.Value, 
                    glAcct.Name, glAcct, date));
            }
            return acctGrp;
        }


        private static GLRecapAllocation CreateAllocation(decimal amount, string label, GLAccountDTO acct, DateTime date)
        {
            var alloc = new AccountAllocation
            {
                Account   = acct,
                SubAmount = amount
            };
            var req = new FundRequestDTO
            {
                Purpose    = label,
                DateOffset = date.DaysSinceMin(),
            };
            return new GLRecapAllocation(alloc, req);
        }
    }
}
