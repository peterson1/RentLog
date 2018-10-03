using System;
using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.AdHocJobs
{
    public class ForAllRequests
    {
        public static void SetDateOffset(ITenantDBsDir dir)
        {
            SetDateOffset(dir.Vouchers.ActiveRequests);
            SetDateOffset(dir.Vouchers.InactiveRequests);
            SetDateOffset(dir.Vouchers.PreparedCheques);
        }


        private static void SetDateOffset(IChequeVouchersRepo chqsRepo)
        {
            var all = chqsRepo.GetAll();

            foreach (var chq in all)
                SetRequestOffset(chq.Request);

            chqsRepo.DropAndInsert(all, false, true);
        }


        private static void SetDateOffset(IFundRequestsRepo reqsRepo)
        {
            var all = reqsRepo.GetAll();

            foreach (var req in all)
                SetRequestOffset(req);

            reqsRepo.DropAndInsert(all, false, true);
        }


        private static void SetRequestOffset(FundRequestDTO req)
            => req.DateOffset = req.RequestDate.DaysSinceMin();
    }
}
