using System;

namespace RentLog.DomainLib11.Models
{
    public class TenantModel
    {
        public string    FirstName    { get; set; }
        public string    MiddleName   { get; set; }
        public string    LastName     { get; set; }
                                      
        public DateTime  BirthDate    { get; set; }
        public string    SssNumber    { get; set; }
        public string    TinNumber    { get; set; }
        public string    SpouseName   { get; set; }
        public string    Remarks      { get; set; }
                                      
        public string    Phone1       { get; set; }
        public string    Phone2       { get; set; }
        public string    Email        { get; set; }
                                      
        public string    LotNumber     { get; set; }
        public string    StreetName    { get; set; }
        public string    Barangay      { get; set; }
        public string    Municipality  { get; set; }
        public string    Province      { get; set; }
        public string    PostalCode    { get; set; }
        public string    Country       { get; set; }

        public string FirstAndLastNames => $"{FirstName} {LastName}";

        public override string ToString() => FirstAndLastNames;

        public TenantModel DeepClone   () => throw new NotImplementedException();
        public TenantModel ShallowClone() => (TenantModel)this.MemberwiseClone();


        public static TenantModel Named(string name)
            => new TenantModel { FirstName = name };
    }
}
