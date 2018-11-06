using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.ByfServerAccess;
using RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.ByfQueries
{
    public static class ByfCheckVouchersByDateQueries
    {
        private const string HEADER_VIEWS = "check_vouchers?display_id=page"; 
        private const string ITEM_VIEWS   = "voucher_items?display_id=page";


        public static async Task<CVsByDateCell> QueryCheckVouchers(this ByfServerVM svr, DateTime date)
        {
            var headrJob  = svr.GetViewsList(HEADER_VIEWS, date);
            var itemsJob  = svr.GetViewsList(ITEM_VIEWS, date);
            await Task.WhenAll(headrJob, itemsJob);
            var byfHeadrs = await headrJob;
            var byfItems  = await itemsJob;
            var fundReqs  = CastAsFundReqs(byfHeadrs, byfItems);
            var checks    = CastAsChecks  (byfHeadrs, byfItems);

            return new CVsByDateCell
            {
                ActiveRequests   = GetActiveRequests  (fundReqs),
                InactiveRequests = GetInactiveRequests(fundReqs),
                RequestedChecks  = GetRequestedChecks (checks),
            };
        }


        private static List<FundRequestDTO> CastAsFundReqs(List<dynamic> byfHeadrs, List<dynamic> byfItems)
        {
            //var allocs = 
            //var list = new List<FundRequestDTO>();
            //
            //return list;
            throw new NotImplementedException();
        }


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
