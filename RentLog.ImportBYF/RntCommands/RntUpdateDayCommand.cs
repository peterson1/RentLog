using RentLog.ImportBYF.Converters.AmbulantColxnConverters;
using RentLog.ImportBYF.Converters.BankDepositConverters;
using RentLog.ImportBYF.Converters.CashierColxnConverters;
using RentLog.ImportBYF.Converters.IntendedColxnConverters;
using RentLog.ImportBYF.Converters.OtherColxnConverters;
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
            await Task.Delay(1);
            await Task.Run(() =>
            {
                UpdateIntendedColxns (row);
                UpdateAmbulantColxns (row);
                UpdateOtherColxns    (row);
                UpdateCashierColxns  (row);
                UpdateBankDeposits   (row);
            });
            row.StopBeingBusy();
            row.RefreshCmd.ExecuteIfItCan();
        }


        private static void UpdateIntendedColxns(DailyTransactionRow row)
            => new IntendedColxnConverter1(row.MainWindow)
                .Rewrite(row.Date);


        private static void UpdateAmbulantColxns(DailyTransactionRow row)
            => new AmbulantColxnConverter1(row.MainWindow)
                .Rewrite(row.Date);


        private static void UpdateOtherColxns(DailyTransactionRow row)
            => new OtherColxnConverter1(row.MainWindow)
                .Rewrite(row.Date);


        private static void UpdateCashierColxns(DailyTransactionRow row)
            => new CashierColxnConverter1(row.MainWindow)
                .Rewrite(row.Date);


        private static void UpdateBankDeposits(DailyTransactionRow row)
            => new BankDepositConverter1(row.MainWindow)
                .Rewrite(row.Date);
    }
}
