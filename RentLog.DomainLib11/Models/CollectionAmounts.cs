namespace RentLog.DomainLib11.Models
{
    public class CollectionAmounts : BillAmounts
    {
        public decimal? Ambulant { get; set; }

        public override decimal Total => base.Total + (Ambulant ?? 0);
    }
}
