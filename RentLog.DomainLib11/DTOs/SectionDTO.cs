using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ReflectionTools;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class SectionDTO : IDocumentDTO, ICloneable
    {
        public int        Id         { get; set; }
        public string     Author     { get; set; }
        public DateTime   Timestamp  { get; set; }
        public string     Remarks    { get; set; }


        public string     Name           { get; set; }
        public StallDTO   StallTemplate  { get; set; }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();

        public override string ToString() => Name;


        public static SectionDTO Named(string name)
            => new SectionDTO { Name = name };
    }
}
