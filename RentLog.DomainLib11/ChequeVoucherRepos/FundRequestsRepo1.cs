using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public class FundRequestsRepo1 : SimpleRepoShimBase<FundRequestDTO>, IFundRequestsRepo
    {
        public FundRequestsRepo1(ISimpleRepo<FundRequestDTO> simpleRepo) : base(simpleRepo)
        {
        }

        public DateTime GetMaxRequestDate() 
            => !Any() ? DateTime.Now : Max(_ => _.RequestDate);


        public int GetMaxSerial() 
            => !Any() ? 0 : Max(_ => _.SerialNum);


        public List<string> GetPayees()
            => GetAll().Select  (_ => _.Payee)
                       .GroupBy (_ => _)
                       .Select  (_ => _.First())
                       .ToList  ();
    }
}
