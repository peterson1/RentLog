using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.DTOs
{
    public class SectionDTO : DocumentDTOBase
    {
        public string     Name           { get; set; }
        public StallDTO   StallTemplate  { get; set; }


        public override string ToString() => Name;


        public static SectionDTO Named(string name)
            => new SectionDTO { Name = name };
    }
}
