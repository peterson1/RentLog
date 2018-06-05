namespace RentLog.DomainLib11.DTOs
{
    public class AmbulantColxnDTO : DocumentDTOBase
    {
        public int?       PRNumber      { get; set; }
        public string     ReceivedFrom  { get; set; }
        public decimal    Amount        { get; set; }


        public override string ToString()
            => $"‹Ambulant Collection› from {ReceivedFrom}";
    }
}
