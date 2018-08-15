using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.AllChequeVouchers
{
    public class FilterVM : TextFilterBase<FundRequestDTO>
    {
        public string  FilterSerialNum       { get; set; }
        public string  FilterRequestDate     { get; set; }
        public string  FilterPayee           { get; set; }
        public string  FilterPurpose         { get; set; }
        public string  FilterAmount          { get; set; }
        public string  FilterDebitAccounts   { get; set; }
        public string  FilterCreditAccounts  { get; set; }


        protected override Dictionary<string, Func<FundRequestDTO, string>> FilterProperties => new Dictionary<string, Func<FundRequestDTO, string>>
        {
            { nameof(FilterSerialNum     ), _ => _.SerialNum.ToString("0000") },
            { nameof(FilterRequestDate   ), _ => _.RequestDate.ToString("d MMM yyyy") },
            { nameof(FilterPayee         ), _ => _.Payee },
            { nameof(FilterPurpose       ), _ => _.Purpose },
            { nameof(FilterAmount        ), _ => _.Amount.ToString() },
            { nameof(FilterDebitAccounts ), _ => _.DebitAccounts },
            { nameof(FilterCreditAccounts), _ => _.CreditAccounts },
        };
    }
}
