using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.ReportRows
{
    public class GLRecapAllocation : AccountAllocation
    {
        public GLRecapAllocation(AccountAllocation alloc, FundRequestDTO req)
        {
            Account   = alloc.Account;
            SubAmount = alloc.SubAmount;
            Request   = req;
        }


        public FundRequestDTO   Request   { get; }
    }
}
