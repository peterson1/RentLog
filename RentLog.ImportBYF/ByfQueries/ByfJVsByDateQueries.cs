using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.RntQueries;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.JournalVouchersPane.JVsByDateList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfJVsByDateQueries
    {
        private const string HEADERS_VIEW = "journal_voucher_headers?display_id=page";
        private const string ITEMS_VIEW   = "journal_voucher_items?display_id=page";


        public static async Task<JVsByDateCell> QueryJournalVouchers(this MainWindowVM2 main, DateTime date)
        {
            var svr       = main.ByfServer;
            var headrJob  = svr.GetViewsList(HEADERS_VIEW, date);
            var itemsJob  = svr.GetViewsList(ITEMS_VIEW, date);
            await Task.WhenAll(headrJob, itemsJob);
            var byfHeadrs = await headrJob;
            var byfItems  = await itemsJob;

            var allByfs   = CastAsAllByfs(byfHeadrs, byfItems, main);
        }


        private static List<JournalVoucherDTO> CastAsAllByfs(List<dynamic> byfHeadrs, List<dynamic> byfItems, MainWindowVM2 main)
        {
            var headrs    = byfHeadrs.Select(_ => (JournalVoucherDTO)CastAsVoucherDTO(_, main.RntCache));
            var itemsDict = GroupItemsByHeader(byfItems, main.ByfCache);

            foreach (var kvp in headrsDict)
            {
                var dto = kvp.Value;
            }
        }


        private static Dictionary<int, List<AccountAllocation>> GroupItemsByHeader(List<dynamic> byfItems, RntCache cache)
        {
            var dict = new Dictionary<int, List<AccountAllocation>>();
            foreach (var byf in byfItems)
            {
                var alloc = CastAsAllocation(byf, cache, out int headrId);

            }
            return dict;
        }


        private static JournalVoucherDTO CastAsVoucherDTO(dynamic byf) => new JournalVoucherDTO
        {
            Id          = As.ID(byf.nid),
            Description = As.Text(byf.title),
            DateOffset  = As.DateOffset(byf.batchdate),
            SerialNum   = As.ID(byf.serial),
            Remarks     = As.Text(byf.remarks),
            Amount      = 0, //todo: set amount
            Allocations = new List<AccountAllocation>(),
        };


        private static AccountAllocation CastAsAllocation(dynamic byf, RntCache cache, out int headrId)
        {
            headrId      = As.ID(byf.headernid);
            var absAmt   = As.Decimal(byf.amount);
            var isCredit = As.Bool_(byf.cr_dr) ?? false;
            return new AccountAllocation
            {
                Account   = cache.GLAcctById(As.ID(byf.glaccountnid)),
                SubAmount = absAmt * (isCredit ? 1M : -1M),
            };
        }
    }
}
