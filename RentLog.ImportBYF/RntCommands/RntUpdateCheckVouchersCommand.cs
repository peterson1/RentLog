using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList;
using System;
using System.Linq;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateCheckVouchersCommand
    {
        public static void UpdateCheckVouchers(this CVsByDateRow row)
        {
            var db      = row.MainWindow.AppArgs.Vouchers;
            var pbk     = row.MainWindow.AppArgs.Passbooks;
            var cache   = row.MainWindow.ByfCache;
            var inactvs = row.ByfCell.InactiveRequests
                             .Select(_ => _.Request).ToList();

            db.ActiveRequests.Delete(row.ByfCell.ActiveRequests);
            db.ActiveRequests.Insert(row.ByfCell.ActiveRequests, true);

            db.InactiveRequests.Delete(inactvs);
            db.InactiveRequests.Insert(inactvs, true);

            db.PreparedCheques.Delete(row.ByfCell.PreparedCheques);
            db.PreparedCheques.Insert(row.ByfCell.PreparedCheques, true);

            foreach (var chk in row.ByfCell.InactiveRequests)
            {
                if (!cache.ClearedDatesById.TryGetValue(chk.Id, 
                    out DateTime cleared)) continue;

                if (chk.Request.ChequeStatus != ChequeState.Cleared) continue;

                var repo   = pbk.GetRepo(chk.Request.BankAccountId);
                var match  = repo.FindByDocRefId(chk.Id, cleared);
                if (match != null) repo.Delete(match);
                repo.InsertClearedCheque(chk, cleared);
            }
        }
    }
}
