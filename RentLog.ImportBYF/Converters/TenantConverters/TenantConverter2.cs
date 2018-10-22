using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.TenantConverters
{
    public static class TenantConverter2
    {
        public const string ViewsDisplayID = "tenants_list?display_id=page_2";


        public static async Task<Dictionary<int, TenantModel>> GetTenantsDictionary (this IConverterRow conv)
        {
            var dynamics = await conv.GetViewsList(ViewsDisplayID);
            var dict     = new Dictionary<int, TenantModel>();

            foreach (var byf in dynamics)
            {
                var nid = As.ID(byf.nid);
                var rnt = CastToRNT(byf);
                dict.Add(nid, rnt);
            }
            return dict;
        }


        private static TenantModel CastToRNT(dynamic byf) => new TenantModel
        {
            FirstName    = As.Text(byf.firstname),
            MiddleName   = As.Text(byf.middlename),
            LastName     = As.Text(byf.lastname),
            LotNumber    = As.Text(byf.housenumber),
            StreetName   = As.Text(byf.street),
            Barangay     = As.Text(byf.barangay),
            Municipality = As.Text(byf.municipality),
            Province     = As.Text(byf.province),
            PostalCode   = As.Text(byf.postalcode),
            Country      = As.Text(byf.country),
            Phone1       = As.Text(byf.phone),
            Phone2       = As.Text(byf.phone2),
            Email        = As.Text(byf.email),
            BirthDate    = As.Date(byf.birthdate),
            SssNumber    = As.Text(byf.sss),
            TinNumber    = As.Text(byf.tin),
            SpouseName   = As.Text(byf.spousename),
            Remarks      = As.Text(byf.remarks),
        };
    }
}
