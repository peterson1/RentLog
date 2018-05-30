using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class OfficeColxnsRow : SectionColxnsRow
    {
        public OfficeColxnsRow(DateTime date, ITenantDBsDir dir) : base(SectionDTO.Named("Office"), date, dir)
        {
        }


        protected override List<LeaseColxnRow> GetLeaseColxns(SectionDTO sec, DateTime date, ICollectionsDB db, ITenantDBsDir dir)
        {
            var colxns = db.CashierColxns.GetAll()
                            .Select(_ => new LeaseColxnRow(_));

            foreach (var item in colxns)
                dir.MarketState.RefreshStall(item.Lease);

            return colxns.ToList();
        }


        protected override CollectorDTO GetSectionCollector(SectionDTO sec, ICollectionsDB db)
            => CollectorDTO.Office();
    }
}
