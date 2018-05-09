using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.CollectionRepos
{
    public class CashierColxnsRepo1 : SimpleRepoShimBase<CashierColxnDTO>, ICashierColxnsRepo
    {
        public CashierColxnsRepo1(ISimpleRepo<CashierColxnDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
