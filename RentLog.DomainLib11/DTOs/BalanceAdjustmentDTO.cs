using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.DTOs
{
    public class BalanceAdjustmentDTO : IDocumentDTO, ICloneable
    {
        public int         Id           { get; set; }
        public string      Author       { get; set; }
        public DateTime    Timestamp    { get; set; }
        public string      Remarks      { get; set; }

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


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
