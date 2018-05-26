using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class UncollectedLeaseDTO : IDocumentDTO, ICloneable
    {
        public int           Id         { get; set; }
        public string        Author     { get; set; }
        public DateTime      Timestamp  { get; set; }
        public string        Remarks    { get; set; }

        public LeaseDTO      Lease      { get; set; }
        public BillAmounts   Targets    { get; set; }

        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
