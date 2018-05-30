using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using RentLog.DomainLib11.Models;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.DTOs
{
    public class CashierColxnDTO : IDocumentDTO, ICloneable
    {
        public int        Id         { get; set; }
        public string     Author     { get; set; }
        public DateTime   Timestamp  { get; set; }
        public string     Remarks    { get; set; }

        public string     DocumentRef  { get; set; }
        public LeaseDTO   Lease        { get; set; }
        public decimal    Amount       { get; set; }
        public BillCode   BillCode     { get; set; }


        public decimal? For(BillCode billCode)
            => billCode == BillCode ? Amount : (decimal?)null;


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();


        internal BillPayment ToBillPayment() => new BillPayment
        {
            Amount    = this.Amount,
            PRNumber  = int.TryParse(DocumentRef, out int num) ? num : -1,
            Remarks   = this.Remarks,
            Collector = CollectorDTO.Office(),
        };
    }
}
