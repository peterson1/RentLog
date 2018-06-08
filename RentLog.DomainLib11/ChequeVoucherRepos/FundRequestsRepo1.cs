using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class FundRequestsRepo1 : SimpleRepoShimBase<FundRequestDTO>, IFundRequestsRepo
    {
        public FundRequestsRepo1(ISimpleRepo<FundRequestDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
