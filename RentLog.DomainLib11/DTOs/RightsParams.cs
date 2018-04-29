namespace RentLog.DomainLib11.DTOs
{
    public class RightsParams
    {
        public decimal   TotalAmount     { get; set; }
        public int       SettlementDays  { get; set; }
        public string    PenaltyRule     { get; set; }
        public decimal   PenaltyRate1    { get; set; }
        public decimal   PenaltyRate2    { get; set; }
    }
}
