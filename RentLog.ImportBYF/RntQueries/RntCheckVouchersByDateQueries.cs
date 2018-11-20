using RentLog.DomainLib11.ChequeVoucherRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntCheckVouchersByDateQueries
    {
        public static CVsByDateCell GetCheckVouchers(this ITenantDBsDir dir, DateTime date)
        {
            var db = dir.Vouchers;
            return new CVsByDateCell
            {
                ActiveRequests   = GetActiveRequests  (db, date),
                InactiveRequests = GetInactiveRequests(db, date),
                RequestedChecks  = GetPreparedCheques (db, date),
            };
        }


        private static List<FundRequestDTO> GetActiveRequests(ChequeVouchersDB db, DateTime date)
            => db.ActiveRequests.GetAll()
                 .Where(_ => _.RequestDate == date)
                 .ToList();


        private static List<FundRequestDTO> GetInactiveRequests(ChequeVouchersDB db, DateTime date)
            => db.InactiveRequests.GetAll()
                 .Where  (_ => _.RequestDate == date)
                 .ToList ();


        private static List<ChequeVoucherDTO> GetPreparedCheques(ChequeVouchersDB db, DateTime date)
            => db.PreparedCheques.GetAll()
                 .Where  (_ => _.Request.RequestDate == date)
                 .ToList ();
    }
}
