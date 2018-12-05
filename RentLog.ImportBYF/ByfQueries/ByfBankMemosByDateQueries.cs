using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfBankMemosByDateQueries
    {
        private const string VIEW_ID = "bank_balance_adjustments?display_id=page";


        public static async Task<MemosByDateCell> QueryBankMemos (this MainWindowVM2 main, DateTime date)
        {
            var byfs = await main.ByfServer.GetViewsList(VIEW_ID, date);
            var dict = new Dictionary<int, List<PassbookRowDTO>>();

            foreach (var byf in byfs)
            {
                (int BankAcctID, PassbookRowDTO DTO) tupl;
                tupl = CastToRnt(byf);
                if (tupl.DTO.Amount == 0M) continue;

                if (!dict.ContainsKey(tupl.BankAcctID))
                    dict[tupl.BankAcctID] = new List<PassbookRowDTO>();

                dict[tupl.BankAcctID].Add(tupl.DTO);
            }
            return new MemosByDateCell { DTOs = dict };
        }


        private static (int BankAcctID, PassbookRowDTO DTO) CastToRnt(dynamic byf)
        {
            var dto = new PassbookRowDTO
            {
                Subject        = As.Text(byf.title),
                Description    = As.Text(byf.remarks),
                TransactionRef = As.Text(byf.referencenum),
                DateOffset     = As.DateOffset(byf.date),
                Amount         = As.Decimal(byf.amount),
                DocRefType     = ByfBankMemoNode.TypeName,
                DocRefId       = As.ID(byf.nid),
                DocRefJson     = "{}",
            };
            if (dto.Subject.IsBlank())
                dto.Subject = "-";
            if (dto.Description.IsBlank())
                dto.Description = "-";
            if (dto.TransactionRef.IsBlank())
                dto.TransactionRef = "-";
            return (As.ID(byf.bankacctnid), dto);
        }
    }
}
