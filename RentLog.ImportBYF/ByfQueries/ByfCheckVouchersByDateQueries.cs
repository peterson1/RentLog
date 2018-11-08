using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.RntQueries;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfCheckVouchersByDateQueries
    {
        private const string HEADER_VIEWS = "check_vouchers?display_id=page"; 
        private const string ITEM_VIEWS   = "voucher_items?display_id=page";


        public static async Task<CVsByDateCell> QueryCheckVouchers(this MainWindowVM2 main, DateTime date)
        {
            var svr       = main.ByfServer;
            var headrJob  = svr.GetViewsList(HEADER_VIEWS, date);
            var itemsJob  = svr.GetViewsList(ITEM_VIEWS, date);
            await Task.WhenAll(headrJob, itemsJob);
            var byfHeadrs = await headrJob;
            var byfItems  = await itemsJob;
            var fundReqs  = CastAsFundReqs(byfHeadrs, byfItems, main);
            var checks    = CastAsChecks  (byfHeadrs, byfItems);

            return new CVsByDateCell
            {
                ActiveRequests   = GetActiveRequests  (fundReqs),
                InactiveRequests = GetInactiveRequests(fundReqs),
                RequestedChecks  = GetRequestedChecks (checks),
            };
        }


        private static List<FundRequestDTO> CastAsFundReqs(List<dynamic> byfHeadrs, List<dynamic> byfItems, MainWindowVM2 main)
        {
            var headrsDict = byfHeadrs.Select(_ => (FundRequestDTO)CastAsFundReqHeader(_, main.ByfCache))
                                      .ToDictionary(_ => _.Id);

            var itemsList  = byfItems.Select(_ => CastAsAllocation(_, main.RntCache)).ToList();

            foreach ((int HeaderId, AccountAllocation Allocation) tupl in itemsList)
                headrsDict[tupl.HeaderId].Allocations.Add(tupl.Allocation);

            foreach (var hdr in headrsDict.Values)
                hdr.BankAccountId = GetBankAcctId(hdr);

            throw new NotImplementedException();
        }


        private static int GetBankAcctId(FundRequestDTO hdr)
        {
            //todo: set Bank Acct ID
            throw new NotImplementedException();
        }


        private static (int HeaderId, AccountAllocation Allocation) CastAsAllocation(dynamic byf, RntCache cache)
        {
            var hdrId    = As.ID(byf.parentnid);
            var isCredit = (int)As.ID(byf.cr_dr) == 1;
            var absAmt   = (decimal)As.Decimal(byf.amount);
            return (hdrId, new AccountAllocation
            {
                SubAmount = absAmt * (isCredit ? 1M : -1M),
                Account   = cache.GLAcctById(byf.glaccountnid)
            });
        }


        private static FundRequestDTO CastAsFundReqHeader(dynamic byf, ByfCache cache)
        {
            var rnt = new FundRequestDTO
            {
                Id           = As.ID(byf.nid),
                Amount       = As.Decimal_(byf.checkamountactual),
                SerialNum    = As.ID(byf.serial),
                DateOffset   = As.DateOffset(byf.requestdate),
                Payee        = (string)As.Text(byf.adhocpayee),
                Purpose      = As.Text(byf.description),
                Remarks      = As.Text(byf.remarks),
                ChequeStatus = GetChequeStatus(byf),
                Allocations  = new List<AccountAllocation>()
            };

            if (rnt.Payee.IsBlank())
                rnt.Payee = cache.PayeeById(As.ID(byf.savedpayeenid));

            return rnt;
        }


        private static ChequeState? GetChequeStatus(dynamic byf)
        {
            if (HasValue(byf.canceleddate)) return ChequeState.Cancelled;
            if (HasValue(byf.cleareddate )) return ChequeState.Cleared;
            if (HasValue(byf.issueddate  )) return ChequeState.Issued;
            return ChequeState.Prepared;
        }


        private static bool HasValue(dynamic byfVal)
            => !((string)As.Text(byfVal)).IsBlank();


        private static List<ChequeVoucherDTO> CastAsChecks(List<dynamic> byfHeadrs, List<dynamic> byfItems)
        {
            var list = new List<ChequeVoucherDTO>();

            return list;
        }


        private static List<FundRequestDTO> GetActiveRequests(List<FundRequestDTO> fundReqs)
        {
            throw new NotImplementedException();
        }


        private static List<FundRequestDTO> GetInactiveRequests(List<FundRequestDTO> fundReqs)
        {
            throw new NotImplementedException();
        }


        private static List<ChequeVoucherDTO> GetRequestedChecks(List<ChequeVoucherDTO> checks)
        {
            throw new NotImplementedException();
        }
    }
}
