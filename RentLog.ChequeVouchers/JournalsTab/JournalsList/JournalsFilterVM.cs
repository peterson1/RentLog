using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsList
{
    public class JournalsFilterVM : TextFilterBase<JournalVoucherDTO>
    {
        protected override Dictionary<string, Func<JournalVoucherDTO, string>> FilterProperties => new Dictionary<string, Func<JournalVoucherDTO, string>>
        {

        };
    }
}
