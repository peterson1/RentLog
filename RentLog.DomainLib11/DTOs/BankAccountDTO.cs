using CommonTools.Lib11.DTOs;

namespace RentLog.DomainLib11.DTOs
{
    public class BankAccountDTO : DocumentDTOBase
    {
        public string    Name                { get; set; }
        public decimal?  MaintainingBalance  { get; set; }

        public override string ToString() => Name;


        public static BankAccountDTO Named(string name)
            => new BankAccountDTO { Name = name };
    }
}
