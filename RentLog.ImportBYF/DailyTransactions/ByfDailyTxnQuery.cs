using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.DailyTransactions
{
    public static class ByfDailyTxnQuery
    {
        public static async Task<DailyTransactionCell> CheckCache(this DailyTransactionRow row, string cacheDir)
        {
            var dates = CacheReader2.GetLoadedDates(cacheDir);
            if (dates.Contains(row.Date))
                return ReadLoadedDate(row.Date, cacheDir);
            else
                return await QueryByfServer(row.Date);
        }


        private static async Task<DailyTransactionCell> QueryByfServer(DateTime date)
        {
            await Task.Delay(1000 * 1);
            //throw new NotImplementedException();
            return new DailyTransactionCell();
        }


        private static DailyTransactionCell ReadLoadedDate(DateTime date, string cacheDir) => new DailyTransactionCell
        {
            TotalCollections = ReadTotalCollections (date, cacheDir),
            TotalDeposits    = ReadTotalDeposits    (date, cacheDir)
        };


        private static decimal ReadTotalCollections(DateTime date, string cacheDir)
        {
            var secColxns = SectionColxnCacheReader2
                    .getAllSectionColxnRecs(cacheDir, date)
                    .Sum(_ => _.Subtotal);

            var othrColxns = OtherColxnCacheReader2.getSourceRecords(cacheDir, date)
                    .Sum(_ => _.Amount);

            return (decimal)(secColxns + othrColxns);
        }

        private static decimal ReadTotalDeposits(DateTime date, string cacheDir)
            => (decimal)BankDepositsCacheReader2
                .getSourceRecords(cacheDir, date)
                    .Sum(_ => _.Amount);
    }
}
