using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class FundRequestsRepo1 : SimpleRepoShimBase<FundRequestDTO>, IFundRequestsRepo
    {
        public FundRequestsRepo1(ISimpleRepo<FundRequestDTO> simpleRepo) : base(simpleRepo)
        {
        }


        public List<string> GetPayees()
            => GetAll().Select  (_ => _.Payee)
                       .GroupBy (_ => _)
                       .Select  (_ => _.First())
                       .ToList  ();
    }
}
