using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.DTOs
{
    public class CollectorDTO : DocumentDTOBase
    {
        public string     Name       { get; set; }
        public bool       IsActive   { get; set; }


        public override string ToString() => Name;


        public static CollectorDTO Named(string name)
            => new CollectorDTO { Name = name };


        public static CollectorDTO Office()
            => CollectorDTO.Named("Office");
    }
}
