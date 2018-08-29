using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.JsonTools;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.Reporters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class GLRecapCategory : UIList<GLRecapAccountGroup>
    {
        public GLRecapCategory(GLAcctType acctTyp, GLRecapReport mainReport)
        {
            AccountType = acctTyp;
            FillWithSubRows(mainReport);
        }


        public GLAcctType  AccountType  { get; }


        public decimal? TotalDebits  => this.Sum(_ => _.TotalDebits);
        public decimal? TotalCredits => this.Sum(_ => _.TotalCredits);


        private void FillWithSubRows(GLRecapReport main)
        {
            var allocs  = new List<GLRecapAllocation>();
            //var allReqs = main.DBsDir.Vouchers.AllRequests.GetAll()
            //                  .Where(_ => IsWithinDates(_, main))
            //                  .ToList();
            var allReqs = GetCombinedList(main);

            foreach (var req in allReqs)
            {
                foreach (var alloc in req.Allocations)
                {
                    if (alloc.Account.AccountType == this.AccountType)
                        allocs.Add(new GLRecapAllocation(alloc, req));
                }
            }
            GroupByAccount(allocs, main);
        }


        private List<FundRequestDTO> GetCombinedList(GLRecapReport main)
        {
            var repos   = main.DBsDir.Vouchers;
            var activs  = GetRequestsWithinDates(main, repos.ActiveRequests);
            var inactvs = GetRequestsWithinDates(main, repos.InactiveRequests);
            return activs.Concat(inactvs).ToList();
        }


        private List<FundRequestDTO> GetRequestsWithinDates(GLRecapReport main, IFundRequestsRepo repo)
        {
            var start = main.StartDate.DaysSinceMin();
            var end   = main.EndDate.DaysSinceMin();
            return repo.Find(_ => _.DateOffset >= start
                               && _.DateOffset <= end);
        }


        private bool IsWithinDates(FundRequestDTO req, GLRecapReport main)
        {
            var date = req.RequestDate.Date;
            if (date < main.StartDate) return false;
            if (date > main.EndDate) return false;
            return true;
        }


        private void GroupByAccount(List<GLRecapAllocation> allocs, GLRecapReport main)
        {
            var grpdByAcct = allocs.GroupBy(_ => _.Account.Name)
                                   .OrderBy(_ => _.Key);
            foreach (var grp in grpdByAcct)
            {
                var acct = GLAccountDTO.Named(grp.Key);
                this.Add(new GLRecapAccountGroup(acct, grp));
            }
        }
    }
}
