using CommonTools.Lib11.DTOs;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public static class LeaseStateSegregator
    {
        //public static (List<LeaseDTO> Actives, List<InactiveLeaseDTO> Inactives) SegregateByTermination(this IEnumerable<IDocumentDTO> records, DateTime lastClosedDate)
        //{
        //    var actives = new List<LeaseDTO>();
        //    var inactvs = new List<InactiveLeaseDTO>();

        //    foreach (LeaseDTO lse in records)
        //    {
        //        if (lse is InactiveLeaseDTO inactv)
        //            inactvs.Add(inactv);
        //        else
        //        {
        //            if (lse.IsActive(lastClosedDate))
        //                actives.Add(lse);
        //            else
        //                inactvs.Add(new InactiveLeaseDTO(lse, "Expired", lse.ContractEnd.Value, "Migrator"));
        //        }
        //    }
        //    return (actives, inactvs);
        //}


        public static IDocumentDTO AsInactiveIfSo (this LeaseDTO lease, IDictionary<long, DateTime> terminations, DateTime lastClosedDate)
        {
            if (terminations == null) return lease;

            if (terminations.TryGetValue(lease.Id, out DateTime termDate))
                return new InactiveLeaseDTO(lease, "Terminated", termDate, "Migrator");

            if (!lease.IsActive(lastClosedDate))
                return new InactiveLeaseDTO(lease, "Expired", lease.ContractEndOrMax, "Migrator");

            return lease;
        }
    }
}
