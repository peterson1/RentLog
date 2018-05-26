using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public enum OtherCode
    {
        CR_Fee         = 1,
        Parking        = 2,
        Processing_Fee = 3,
        Other          = 4
    }

    public class OtherColxnDTO : IDocumentDTO, ICloneable
    {
        public int        Id         { get; set; }
        public string     Author     { get; set; }
        public DateTime   Timestamp  { get; set; }
        public string     Remarks    { get; set; }

        public string        DocumentRef  { get; set; }
        public CollectorDTO  Collector    { get; set; }
        public decimal       Amount       { get; set; }
        public OtherCode     OtherCode    { get; set; }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
