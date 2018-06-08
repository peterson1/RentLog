using CommonTools.Lib11.DTOs;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class ChequeVoucherDTO : DocumentDTOBase
    {
        public FundRequestDTO  Request       { get; set; }
        public DateTime        ChequeDate    { get; set; }
        public int             ChequeNumber  { get; set; }
        public string          IssuedTo      { get; set; }
        public DateTime?       IssuedDate    { get; set; }
    }
}
