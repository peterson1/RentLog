using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Converters.LeaseConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Converters.StallConverters
{
    public static class LastStallUsageFinder
    {
        public static LeaseDTO FindLatestOccupancy(this List<ReportModels.Lease> byfLeases, ReportModels.Stall stall, LeaseConverter1 lseConv)
        {
            var latest = byfLeases.Where   (_ => _.Stall.Id.Value == stall.Id.Value)
                                .OrderBy (_ => _.Id.Value)
                                .LastOrDefault();

            if (latest == null) return null;

            return lseConv.CastByfToDTO(latest) as LeaseDTO;
        }
    }
}
