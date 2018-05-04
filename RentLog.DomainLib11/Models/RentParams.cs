namespace RentLog.DomainLib11.Models
{
    public enum BillInterval
    {
        Free,
        One_Time,
        Daily,
        Weekly,
        Two_per_Month,
        Monthly,
        Yearly,
        Other
    }


    public class RentParams
    {
        public BillInterval  Interval         { get; set; }
        public decimal       RegularRate      { get; set; }
        public int           GracePeriodDays  { get; set; }
        public string        PenaltyRule      { get; set; }
        public decimal       PenaltyRate1     { get; set; }
        public decimal       PenaltyRate2     { get; set; }
    }
}
