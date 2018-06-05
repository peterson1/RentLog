using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.DTOs
{
    public class StallDTO : DocumentDTOBase
    {
        public string        Name           { get; set; }
        public SectionDTO    Section        { get; set; }
        public bool          IsOperational  { get; set; }
        public RentParams    DefaultRent    { get; set; }
        public RightsParams  DefaultRights  { get; set; }


        public override string ToString() => Name;


        public static StallDTO Named(string name)
            => new StallDTO { Name = name };
    }
}
