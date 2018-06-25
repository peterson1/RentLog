using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsList
{
    public class JournalsFilterVM : TextFilterBase<JournalVoucherDTO>
    {
        public string  FilterSerialNum        { get; set; }
        public string  FilterTransactionDate  { get; set; }
        public string  FilterDescription      { get; set; }
        public string  FilterAmount           { get; set; }
        public string  FilterRemarks          { get; set; }


        protected override Dictionary<string, Func<JournalVoucherDTO, string>> FilterProperties => new Dictionary<string, Func<JournalVoucherDTO, string>>
        {
            { nameof(FilterSerialNum      ), _ => _.SerialNum      .ToString()             },
            { nameof(FilterTransactionDate), _ => _.TransactionDate.ToString("d MMM yyyy") },
            { nameof(FilterDescription    ), _ => _.Description                            },
            { nameof(FilterAmount         ), _ => _.Amount         .ToString()             },
            { nameof(FilterRemarks        ), _ => _.Remarks                                },
        };
    }
}
