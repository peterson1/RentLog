using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.DTOs
{
    public class CollectorDTO : DocumentDTOBase
    {
        public string     Name       { get; set; }
        public bool       IsActive   { get; set; }


        public override string ToString() => Name;


        public static CollectorDTO Named(string name, bool isActive = false)
            => new CollectorDTO
            {
                Name     = name,
                IsActive = isActive
            };


        public static CollectorDTO Office(bool isActive = false)
            => CollectorDTO.Named("Office", isActive);
    }
}
