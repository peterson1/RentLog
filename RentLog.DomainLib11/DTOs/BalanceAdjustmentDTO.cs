using RentLog.DomainLib11.Models;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.DTOs
{
    public class BalanceAdjustmentDTO : DocumentDTOBase
    {
        public BillCode    BillCode      { get; set; }
        public int         LeaseId       { get; set; }
        public decimal     AmountOffset  { get; set; }
        public string      Reason        { get; set; }
        public string      DocumentRef   { get; set; }


        internal BillAdjustment ToBillAdjustment() => new BillAdjustment
        {
            DocumentRef  = this.DocumentRef,
            AdjustedBy   = this.Author,
            AmountOffset = this.AmountOffset,
            Reason       = this.Reason,
        };
    }
}
