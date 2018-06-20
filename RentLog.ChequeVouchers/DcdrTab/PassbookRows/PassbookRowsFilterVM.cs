using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.DcdrTab.PassbookRows
{
    public class PassbookRowsFilterVM : TextFilterBase<PassbookRowDTO>
    {
        public string FilterSubject        { get; set; }
        public string FilterDescription    { get; set; }
        public string FilterDateOffset     { get; set; }
        public string FilterTransactionRef { get; set; }
        public string FilterDeposit        { get; set; }
        public string FilterWithdrawal     { get; set; }
        public string FilterRunningBalance { get; set; }

        protected override Dictionary<string, Func<PassbookRowDTO, string>> FilterProperties => new Dictionary<string, Func<PassbookRowDTO, string>>
        {
            { nameof(FilterSubject       ), _ => _.Subject },
            { nameof(FilterDescription   ), _ => _.Description },
            { nameof(FilterDateOffset    ), _ => _.TransactionDate.ToString("d MMM") },
            { nameof(FilterTransactionRef), _ => _.TransactionRef },
            { nameof(FilterDeposit       ), _ => _.Deposit.ToString() },
            { nameof(FilterWithdrawal    ), _ => _.Withdrawal.ToString() },
            { nameof(FilterRunningBalance), _ => _.RunningBalance.ToString() },
        };
    }
}
