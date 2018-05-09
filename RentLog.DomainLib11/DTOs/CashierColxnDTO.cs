using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using System;

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


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
