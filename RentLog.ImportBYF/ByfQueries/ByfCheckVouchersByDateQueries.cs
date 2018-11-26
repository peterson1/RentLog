using CommonTools.Lib11.CollectionTools;
using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.ExceptionTools;
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

            var allByfs   = CastAsAllByfs (byfHeadrs, byfItems, main);
            //var fundReqs  = CastAsFundReqs(byfHeadrs, byfItems, main);
            //var checks    = CastAsChecks  (byfHeadrs, byfItems, fundReqs);
            var fundReqs  = allByfs.Where  (_ => _.ChequeNumber == 0)
                                   .Select (_ => _.Request)
                                   .ToList ();

            var prepareds = allByfs.Where  (_ => _.ChequeNumber != 0)
                                   .ToList ();

            return new CVsByDateCell
            {
                ActiveRequests   = GetActiveRequests   (fundReqs),
                InactiveRequests = GetInactiveRequests (allByfs),
                PreparedCheques  = GetRequestedChecks  (prepareds),
            };
        }


        private static List<ChequeVoucherDTO> CastAsAllByfs(List<dynamic> byfHeadrs, List<dynamic> byfItems, MainWindowVM2 main)
        {
            var headrsDict = byfHeadrs.Select(_ => (ChequeVoucherDTO)CastAsCheckDTO(_, main.ByfCache))
                                      .ToDictionary(_ => _.Id);

            var itemsList  = byfItems.Select(_ => CastAsAllocation(_, main.RntCache)).ToList();

            foreach ((int HeaderId, AccountAllocation Allocation) tupl in itemsList)
            {
                if (tupl.HeaderId == 0) continue;
                headrsDict[tupl.HeaderId].Request.Allocations.Add(tupl.Allocation);
            }

            foreach (var hdr in headrsDict.Values)
                hdr.Request.BankAccountId = GetBankAcctId(hdr.Request, main);

            return headrsDict.Values
                             .Where  (_ => _.Request.BankAccountId != 0)
                             .ToList ();
        }


        private static int GetBankAcctId(FundRequestDTO hdr, MainWindowVM2 main)
        {
            var cache = main.ByfCache;
            foreach (var row in hdr.Allocations)
            {
                //if (row.IsDebit) continue;
                var glAcct     = row.Account.Id;
                var bnkAcctId  = cache.BankAcctByGlAcct.GetOrDefault(glAcct);
                if (bnkAcctId != 0) return bnkAcctId;
            }
            //throw Bad.Data($"No valid bank acct row from voucher items [hdr:{hdr.Id}].");
            return 0;
        }


        private static (int HeaderId, AccountAllocation Allocation) CastAsAllocation(dynamic byf, RntCache cache)
        {
            var hdrId    = As.ID_(byf.parentnid) ?? 0;
            var isCredit = (int)As.ID(byf.cr_dr) == 1;
            var absAmt   = (decimal)As.Decimal(byf.amount);
            var glAcctId = As.ID(byf.glaccountnid);
            return (hdrId, new AccountAllocation
            {
                SubAmount = absAmt * (isCredit ? 1M : -1M),
                Account   = cache.GLAcctById(glAcctId)
            });
        }


        private static ChequeVoucherDTO CastAsCheckDTO(dynamic byf, ByfCache cache)
        {
            var req = new FundRequestDTO
            {
                Id           = As.ID(byf.nid),
                Amount       = As.Decimal_(byf.checkamountactual),
                SerialNum    = As.ID_(byf.serial) ?? 0,
                DateOffset   = As.DateOffset(byf.requestdate),
                Payee        = (string)As.Text(byf.adhocpayee),
                Purpose      = As.Text(byf.description),
                Remarks      = As.Text(byf.remarks),
                ChequeStatus = GetChequeStatus(byf),
                Allocations  = new List<AccountAllocation>()
            };

            var cleared = (DateTime?)As.Date_(byf.cleareddate);
            if (cleared.HasValue)
                cache.ClearedDatesById[req.Id] = cleared.Value;

            if (req.Payee.IsBlank())
                req.Payee = cache.PayeeById(As.ID(byf.savedpayeenid));

            return new ChequeVoucherDTO
            {
                Id           = req.Id,
                Request      = req,
                ChequeDate   = As.Date_(byf.checkdate) ?? DateTime.MinValue,
                ChequeNumber = As.ID_(byf.checknumber) ?? 0,
                IssuedDate   = As.Date_(byf.issueddate),
                IssuedTo     = As.Text(byf.issuedto),
                Remarks      = req.Remarks,
            };
        }


        private static ChequeState? GetChequeStatus(dynamic byf)
        {
            if (HasValue(byf.canceleddate)) return ChequeState.Cancelled;
            if (HasValue(byf.cleareddate )) return ChequeState.Cleared;
            if (HasValue(byf.issueddate  )) return ChequeState.Issued;
            if (HasValue(byf.checkdate   )) return ChequeState.Prepared;
            return null;
        }


        private static bool HasValue(dynamic byfVal)
            => !((string)As.Text(byfVal)).IsBlank();


        //private static List<ChequeVoucherDTO> CastAsChecks(
        //    List<dynamic> byfHeadrs, List<dynamic> byfItems, 
        //    List<FundRequestDTO> fundReqs)
        //{
        //    var list = new List<ChequeVoucherDTO>();
        //    foreach (var req in fundReqs)
        //    {
        //        var chq = 
        //    }
        //    return list;
        //}


        private static List<FundRequestDTO> GetActiveRequests(List<FundRequestDTO> fundReqs) 
            => fundReqs.Where(_ => !_.ChequeStatus.HasValue).ToList();


        private static List<ChequeVoucherDTO> GetInactiveRequests(List<ChequeVoucherDTO> checks)
            => checks.Where(_ => _.Request.ChequeStatus == ChequeState.Cleared 
                              || _.Request.ChequeStatus == ChequeState.Cancelled).ToList();


        private static List<ChequeVoucherDTO> GetRequestedChecks(List<ChequeVoucherDTO> checks)
            => checks.Where(_ => _.Request              != null
                              && _.Request.ChequeStatus != ChequeState.Cleared
                              && _.Request.ChequeStatus != ChequeState.Cancelled)
                     .ToList();
    }
}
