using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.StallConverters
{
    public static class LastStallUsageFinder
    {
        public static LeaseDTO FindLatestOccupancy(this List<ReportModels.Lease> byfLeases, ReportModels.Stall stall)
        {
            throw new NotImplementedException();
        }
    }
}
