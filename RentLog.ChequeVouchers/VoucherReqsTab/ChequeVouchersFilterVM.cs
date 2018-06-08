using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.VoucherReqsTab
{
    public class ChequeVouchersFilterVM : TextFilterBase<ChequeVoucherDTO>
    {
        public string   FilterSerialNum      { get; set; }
        public string   FilterPayee          { get; set; }
        public string   FilterPurpose        { get; set; }
        public string   FilterRequestDate    { get; set; }
        public string   FilterAmount         { get; set; }
        public string   FilterChequeDate     { get; set; }
        public string   FilterChequeNumber   { get; set; }
        public string   FilterIssuedTo       { get; set; }
        public string   FilterIssuedDate     { get; set; }


        protected override Dictionary<string, Func<ChequeVoucherDTO, string>> FilterProperties => new Dictionary<string, Func<ChequeVoucherDTO, string>>
        {
            { nameof(FilterSerialNum    ), _ => _.Request.SerialNum.ToString()              },
            { nameof(FilterPayee        ), _ => _.Request.Payee                             },
            { nameof(FilterPurpose      ), _ => _.Request.Purpose                           },
            { nameof(FilterRequestDate  ), _ => _.Request.RequestDate.ToString("d MMM yyyy")},
            { nameof(FilterAmount       ), _ => _.Request.Amount?.ToString()                },
            { nameof(FilterChequeDate   ), _ => _.ChequeDate.ToString("d MMM yyyy")},
            { nameof(FilterChequeNumber ), _ => _.ChequeNumber.ToString()},
            { nameof(FilterIssuedTo     ), _ => _.IssuedTo},
            { nameof(FilterIssuedDate   ), _ => _.IssuedDate?.ToString("d MMM yyyy")},
        };
    }
}
