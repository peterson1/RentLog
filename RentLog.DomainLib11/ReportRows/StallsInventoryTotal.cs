using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class StallsInventoryTotal : StallsInventoryRow
    {
        public StallsInventoryTotal(IEnumerable<StallsInventoryRow> rows)
        {
            Section = SectionDTO.Named("total");
            Occupieds.SetItems(rows.SelectMany(_ => _.Occupieds));
            Vacants  .SetItems(rows.SelectMany(_ => _.Vacants));
        }
    }
}
