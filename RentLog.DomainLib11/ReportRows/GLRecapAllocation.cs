using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;

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

        //public GLRecapAllocation(PassbookRowDTO passbookRowDTO, ChequeVoucherDTO chequeVoucherDTO, AccountAllocation vouchrAllocation)
        //{
        //    Account     = vouchrAllocation.Account;
        //    SubAmount   = vouchrAllocation.SubAmount;
        //    PassbookRow = passbookRowDTO;
        //    Voucher     = chequeVoucherDTO;
        //}

        //public PassbookRowDTO   PassbookRow { get; }
        //public ChequeVoucherDTO Voucher     { get; }
        //public FundRequestDTO   Request     => Voucher.Request;
        public FundRequestDTO   Request   { get; }
    }
}
