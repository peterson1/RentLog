using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ReflectionTools;
using RentLog.DomainLib11.Models;
using System;

namespace RentLog.DomainLib11.DTOs
{
    public class StallDTO : IDocumentDTO, ICloneable
    {
        public int        Id           { get; set; }
        public string     Author       { get; set; }
        public DateTime   Timestamp    { get; set; }
        public string     Remarks      { get; set; }


        public string        Name           { get; set; }
        public SectionDTO    Section        { get; set; }
        public bool          IsOperational  { get; set; }
        public RentParams    DefaultRent    { get; set; }
        public RightsParams  DefaultRights  { get; set; }


        public T DeepClone   <T>() => throw new NotImplementedException();
        public T ShallowClone<T>() => (T)this.MemberwiseClone();
        public override string ToString() => Name;
    }
}
