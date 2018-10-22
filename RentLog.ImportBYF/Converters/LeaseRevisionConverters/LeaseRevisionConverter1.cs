using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.StateTransitions;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.LeaseRevisionConverters
{
    public static class LeaseRevisionConverter1
    {
        public const string ViewsDisplayID = "lease_revisions?display_id=page_1";


        public static async Task<Dictionary<int, DateTime>> GetTerminationDates(this IConverterRow conv)
        {
            var dynamics = await conv.GetViewsList(ViewsDisplayID);
            var dict     = new Dictionary<int, DateTime>();

            foreach (var byf in dynamics)
            {
                var modTyp  = As.ID(byf.modtype);
                if (modTyp != 90) continue;

                var lseNid   = As.ID(byf.leasenid);
                var termDate = As.Date(byf.date);

                //if (dict.ContainsKey(lseNid))
                //    throw Bad.Data($"Duplicate lease nid: {lseNid}");
                //
                //dict.Add(lseNid, termDate);
                dict[lseNid] = termDate;
            }
            return dict;
        }


        public static LeaseDTO AsInactiveIfSo (this LeaseDTO lease, Dictionary<int, DateTime> termDates, DateTime lastClosedDate)
        {
            if (termDates == null) return lease;

            if (termDates.TryGetValue(lease.Id, out DateTime termDate))
                return new InactiveLeaseDTO(lease, "Terminated", termDate, "Migrator");

            if (!lease.IsActive(lastClosedDate))
                return new InactiveLeaseDTO(lease, "Expired", lease.ContractEndOrMax, "Migrator");

            return lease;
        }
    }
}
