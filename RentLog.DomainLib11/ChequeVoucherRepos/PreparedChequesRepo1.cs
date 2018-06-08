using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class PreparedChequesRepo1 : SimpleRepoShimBase<ChequeVoucherDTO>, IChequeVouchersRepo
    {
        public PreparedChequesRepo1(ISimpleRepo<ChequeVoucherDTO> simpleRepo) : base(simpleRepo)
        {
        }
    }
}
