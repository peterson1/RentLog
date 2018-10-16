using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    //public static class DeactivationConverter1
    //{
    //    public static (List<LeaseDTO> Actives, List<InactiveLeaseDTO> Inactives) SegregateByTermination(this IEnumerable<IDocumentDTO> records, string cacheDir)
    //    {
    //        var actives = new List<LeaseDTO>();
    //        var inactvs = new List<InactiveLeaseDTO>();
    //        var lseRevs = CacheReader2.getLeaseTerminations(cacheDir);

    //        foreach (LeaseDTO lse in records)
    //        {
    //            if (lseRevs.TryGetValue(lse.Id, out DateTime termDate))
    //                inactvs.Add(new InactiveLeaseDTO(lse, "Terminated", termDate, "Migrator"));
    //            else
    //                actives.Add(lse);
    //        }
    //        return (actives, inactvs);
    //    }
    //}
}
