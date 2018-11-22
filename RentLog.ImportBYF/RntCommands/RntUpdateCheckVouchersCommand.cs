using RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateCheckVouchersCommand
    {
        public static void UpdateCheckVouchers(this CVsByDateRow row)
        {
            var db = row.MainWindow.AppArgs.Vouchers;

            db.ActiveRequests.Delete(row.ByfCell.ActiveRequests);
            db.ActiveRequests.Insert(row.ByfCell.ActiveRequests, true);

            db.InactiveRequests.Delete(row.ByfCell.InactiveRequests);
            db.InactiveRequests.Insert(row.ByfCell.InactiveRequests, true);

            db.PreparedCheques.Delete(row.ByfCell.PreparedCheques);
            db.PreparedCheques.Insert(row.ByfCell.PreparedCheques, true);

            //todo: insert cleared check to passbook
            foreach (var req in collection)
            {

            }
        }
    }
}
