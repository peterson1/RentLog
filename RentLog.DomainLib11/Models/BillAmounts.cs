namespace RentLog.DomainLib11.Models
{
    public class BillAmounts
    {
        public decimal?  Rent      { get; set; }
        public decimal?  Rights    { get; set; }
        public decimal?  Electric  { get; set; }
        public decimal?  Water     { get; set; }

        public virtual decimal Total => (Rent     ?? 0) 
                                      + (Rights   ?? 0) 
                                      + (Electric ?? 0) 
                                      + (Water    ?? 0);

        public bool HasValue => Total != 0M;


        public decimal? For(BillCode billCode)
        {
            switch (billCode)
            {
                case BillCode.Rent    : return Rent    ;
                case BillCode.Rights  : return Rights  ;
                case BillCode.Electric: return Electric;
                case BillCode.Water   : return Water   ;
                default               : return null    ;
            }
        }
    }
}
