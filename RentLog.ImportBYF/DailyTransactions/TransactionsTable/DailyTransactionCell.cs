namespace RentLog.ImportBYF.DailyTransactions
{
    public class DailyTransactionCell
    {
        public decimal  TotalCollections  { get; set; }
        public decimal  TotalDeposits     { get; set; }

        public bool HasValue => (TotalCollections + TotalDeposits) != 0;

        public bool IsBalanced => HasValue 
                               && TotalCollections == TotalDeposits;

        public decimal TotalDiff => TotalDeposits - TotalCollections;
    }
}
