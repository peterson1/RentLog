namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerfSubRowsTotal : CollectorPerfSubRow
    {
        public CollectorPerfSubRowsTotal(CollectorPerformanceRow row) : base()
        {
            Rent = new CollectorPerfCell()
            {
                Target   = row.RentBill.Target,
                Actual   = row.RentBill.Actual,
                NoExcess = row.RentBill.NoExcess,
                Overage  = row.RentBill.OverpaidTotal,
                Shortage = row.RentBill.UnderpaidTotal
            };

            Rights = new CollectorPerfCell()
            {
                Target   = row.RightsBill.Target,
                Actual   = row.RightsBill.Actual,
                NoExcess = row.RightsBill.NoExcess,
                Overage  = row.RightsBill.OverpaidTotal,
                Shortage = row.RightsBill.UnderpaidTotal
            };
        }
    }
}
