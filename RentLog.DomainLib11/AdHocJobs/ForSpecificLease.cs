using RentLog.DomainLib11.DataSources;
using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForSpecificLease
    {
        public static void RebuildSoaFrom(DateTime minDate, int leaseId, ITenantDBsDir dir)
            => dir.Balances.GetRepo(leaseId).RecomputeFrom(minDate);


        public static Action<LeaseDTO, ITenantDBsDir> FindMemosFrom(DateTime startDate, out string taskDescription)
        {
            taskDescription = "Find Memos starting from ...";
            return (lse, dir) =>
            {
                var endDate = lse.ContractStart;
                var bals    = dir.MarketState.Balances.GetRepo(lse.Id);
                var secId   = lse.Stall.Section.Id;
                foreach (var date in startDate.EachDayUpTo(endDate))
                {
                    var colxns  = dir.Collections.For(date);

                    var intends = colxns.IntendedColxns[secId].GetAll();
                    
                    var memos   = colxns.BalanceAdjs.GetAll();
                    var cashrs  = colxns.CashierColxns.GetAll();

                    if (intends.Any(_ => _.Lease.Id == lse.Id)
                     || memos  .Any(_ => _.LeaseId  == lse.Id)
                     || cashrs .Any(_ => _.Lease.Id == lse.Id))
                        bals.ProcessBalancedDay(date);
                }
            };
        }
    }
}
