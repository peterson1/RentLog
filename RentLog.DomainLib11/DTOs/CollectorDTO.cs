using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class CollectorDTO : IDocumentDTO, ICloneable
    {
        public int        Id         { get; set; }
        public string     Author     { get; set; }
        public DateTime   Timestamp  { get; set; }
        public string     Remarks    { get; set; }

        public string     Name       { get; set; }
        public bool       IsActive   { get; set; }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();

        public override string ToString() => Name;


        public static CollectorDTO Named(string name)
            => new CollectorDTO { Name = name };
    }
}
