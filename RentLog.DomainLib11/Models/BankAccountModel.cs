using System;

namespace RentLog.DomainLib11.Models
{
    public class BankAccountModel
    {
        public int       Id         { get; set; }
        public string    Name       { get; set; }
        public string    Author     { get; set; }
        public DateTime  Timestamp  { get; set; }

        public override string ToString() => Name;
    }
}
