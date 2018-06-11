using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public interface IFundRequestsRepo : ISimpleRepo<FundRequestDTO>
    {
        List<string>  GetPayees         ();
        int           GetMaxSerial      ();
        DateTime      GetMaxRequestDate ();
    }
}
