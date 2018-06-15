using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;

namespace RentLog.DomainLib11.ChequeVoucherRepos
{
    public interface IChequeVouchersRepo : ISimpleRepo<ChequeVoucherDTO>
    {
        List<ChequeVoucherDTO>  GetNonIssuedCheques (int bankAcctID);
        List<ChequeVoucherDTO>  GetIssuedCheques    (int bankAcctID);
    }
}
