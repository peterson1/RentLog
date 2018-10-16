using RentLog.DomainLib11.Models;
using CommonTools.Lib11.StringTools;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public static class TenantsConverter1
    {
        public static TenantModel CastTenant(this ReportModels.Tenant byf) => new TenantModel
        {
            FirstName    = byf.FirstName   .Trim().NullIfBlank(),
            MiddleName   = byf.MiddleName  .Trim().NullIfBlank(),
            LastName     = byf.LastName    .Trim().NullIfBlank(),
            BirthDate    = byf.Birthdate,
            SpouseName   = byf.SpouseName  .Trim().NullIfBlank(),
            Email        = byf.Email       .Trim().NullIfBlank(),
            Phone1       = byf.Phone       .Trim().NullIfBlank(),
            Phone2       = byf.Phone2      .Trim().NullIfBlank(),
            SssNumber    = byf.SSS         .Trim().NullIfBlank(),
            TinNumber    = byf.TIN         .Trim().NullIfBlank(),
            LotNumber    = byf.HouseNumber .Trim().NullIfBlank(),
            StreetName   = byf.Street      .Trim().NullIfBlank(),
            Barangay     = byf.Barangay    .Trim().NullIfBlank(),
            Municipality = byf.Municipality.Trim().NullIfBlank(),
            Province     = byf.Province    .Trim().NullIfBlank(),
            Country      = byf.Country     .Trim().NullIfBlank(),
            PostalCode   = byf.PostalCode  .Trim().NullIfBlank(),
            Remarks      = byf.Remarks     .Trim().NullIfBlank(),
        };
    }
}
