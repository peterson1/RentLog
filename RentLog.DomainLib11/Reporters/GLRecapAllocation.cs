using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.Reporters
{
    public class GLRecapAllocation : AccountAllocation
    {
        public GLRecapAllocation(PassbookRowDTO passbookRowDTO, ChequeVoucherDTO chequeVoucherDTO, AccountAllocation vouchrAllocation)
        {
            Account     = vouchrAllocation.Account;
            SubAmount   = vouchrAllocation.SubAmount;
            PassbookRow = passbookRowDTO;
            Voucher     = chequeVoucherDTO;
        }

        public PassbookRowDTO   PassbookRow { get; }
        public ChequeVoucherDTO Voucher     { get; }
        public FundRequestDTO   Request     => Voucher.Request;
    }
}
