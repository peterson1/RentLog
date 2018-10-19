using RentLog.ImportBYF.Converters.IntendedColxnConverters;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.DailyTransactions
{
    public static class RntUpdateDayCommand
    {
        public static bool CanUpdateRnt(this DailyTransactionRow row)
        {
            if (row.IsBusy) return false;
            if (row.ByfCell == null) return false;
            if (!row.ByfCell.HasValue) return false;
            if (!row.ByfCell.IsBalanced) return false;
            return true;
        }


        public static async Task UpdateRnt(this DailyTransactionRow row)
        {
            row.StartBeingBusy("Updating RNT ...");
            var intentedColxnsCnv = new IntendedColxnConverter1(row.MainWindow);
            await Task.Delay(1);
            await Task.Run(() =>
            {
                intentedColxnsCnv.Rewrite(row.Date);
            });
            row.RefreshCmd.ExecuteIfItCan();
        }
    }
}
