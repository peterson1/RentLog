using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
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
                            .Select(_ => ToLeaseColxnRow(_));

            foreach (var item in colxns)
                dir.MarketState.RefreshStall(item.Lease);

            return colxns.ToList();
        }


        private LeaseColxnRow ToLeaseColxnRow(CashierColxnDTO dto) => new LeaseColxnRow
        {
            Lease       = dto.Lease                 ,
            DocumentRef = dto.DocumentRef           ,
            Rent        = dto.For(BillCode.Rent    ),
            Rights      = dto.For(BillCode.Rights  ),
            Electric    = dto.For(BillCode.Electric),
            Water       = dto.For(BillCode.Water   ),
            Ambulant    = dto.For(BillCode.Other   ),
            Remarks     = dto.Remarks               ,
        };


        protected override CollectorDTO GetSectionCollector(SectionDTO sec, ICollectionsDB db)
            => CollectorDTO.Office();
    }
}
