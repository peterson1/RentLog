using RentLog.ImportBYF.Version2UI.JournalVouchersPane.JVsByDateList;

namespace RentLog.ImportBYF.RntCommands
{
    public static class RntUpdateJVsCommand
    {
        public static void UpdateJournalVouchers(this JVsByDateRow row)
        {
            var repo = row.MainWindow.AppArgs.Journals;
            foreach (var jv in row.ByfCell.DTOs)
            {
                var shard = repo.GetSoloShard(jv.TransactionDate);
                shard.Delete(jv.Id);
                shard.Insert(jv);
            }
        }
    }
}
