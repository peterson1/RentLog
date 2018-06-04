using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class SectionColxnsRow : CollectionAmounts
    {
        public SectionColxnsRow(SectionDTO sec, DateTime date, ITenantDBsDir dir)
        {
            Section    = sec;
            var db     = dir.Collections.For(date);
            Collector  = GetSectionCollector(sec, db);
            var colxns = GetLeaseColxns(sec, date, db, dir);
            Rent       = colxns.Sum(_ => _.Rent ?? 0);
            Rights     = colxns.Sum(_ => _.Rights ?? 0);
            Electric   = colxns.Sum(_ => _.Electric ?? 0);
            Water      = colxns.Sum(_ => _.Water ?? 0);
            Ambulant   = colxns.Sum(_ => _.Ambulant ?? 0);

            Details.SetItems(colxns);
        }

        public SectionDTO    Section    { get; set; }
        public CollectorDTO  Collector  { get; set; }

        public UIList<LeaseColxnRow>  Details  { get; } = new UIList<LeaseColxnRow>();


        protected virtual CollectorDTO GetSectionCollector(SectionDTO sec, ICollectionsDB db)
            => db.GetCollector(sec);


        protected virtual List<LeaseColxnRow> GetLeaseColxns(SectionDTO sec, DateTime date, ICollectionsDB db, ITenantDBsDir dir)
        {
            var fromLses = db.IntendedColxns[sec.Id].GetAll()
                             .Select(_ => new LeaseColxnRow(_));

            foreach (var item in fromLses)
                dir.MarketState.RefreshStall(item.Lease);

            var fromAmbu = db.AmbulantColxns[sec.Id].GetAll()
                             .Select(_ => new LeaseColxnRow(Section, _));

            return fromLses.Concat(fromAmbu).ToList();
        }
    }
}
