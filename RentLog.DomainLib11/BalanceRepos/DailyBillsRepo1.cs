using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.BalanceRepos
{
    public class DailyBillsRepo1 : SimpleRepoShimBase<DailyBillDTO>, IDailyBillsRepo
    {
        public DailyBillsRepo1(ISimpleRepo<DailyBillDTO> simpleRepo) : base(simpleRepo)
        {
        }


        protected override IOrderedEnumerable<DailyBillDTO> ToSorted(IEnumerable<DailyBillDTO> items)
            => items.OrderByDescending(_ => _.Id);
    }
}
