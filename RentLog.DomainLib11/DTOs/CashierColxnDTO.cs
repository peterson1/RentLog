using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.Models;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.DTOs
{
    public class CashierColxnDTO : DocumentDTOBase
    {
        public string     DocumentRef  { get; set; }
        public LeaseDTO   Lease        { get; set; }
        public decimal    Amount       { get; set; }
        public BillCode   BillCode     { get; set; }


        public decimal? For(BillCode billCode)
            => billCode == BillCode ? Amount : (decimal?)null;


        internal BillPayment ToBillPayment() => new BillPayment
        {
            Amount    = this.Amount,
            PRNumber  = int.TryParse(DocumentRef, out int num) ? num : -1,
            Remarks   = this.Remarks,
            Collector = CollectorDTO.Office(),
        };


        public override string ToString()
            => $"‹CashierColxn› from [{Lease}]";
    }
}
