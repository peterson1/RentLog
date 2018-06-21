using CommonTools.Lib11.ReflectionTools;
using System;

namespace CommonTools.Lib11.DTOs
{
    public class DocumentDTOBase : IDocumentDTO, ICloneable
    {
        public int       Id         { get; set; }
        public string    Author     { get; set; }
        public DateTime  Timestamp  { get; set; }
        public string    Remarks    { get; set; }


        public override string ToString() => $"‹{GetType().Name}›";


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
    }
}
