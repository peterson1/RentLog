using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerfSubRow
    {
        private IntendedColxnDTO _colxn;

        public CollectorPerfSubRow(IntendedColxnDTO colxn, SectionDTO sectionDTO)
        {
            _colxn  = colxn;
            Section = sectionDTO;
            Rent    = CreateCell(colxn, BillCode.Rent);
            Rent    = CreateCell(colxn, BillCode.Rights);
        }


        public SectionDTO         Section  { get; }
        public CollectorPerfCell  Rent     { get; }
        public CollectorPerfCell  Rights   { get; }

        public LeaseDTO  Lease  => _colxn.Lease;
        public StallDTO  Stall  => Lease?.Stall;


        private CollectorPerfCell CreateCell(IntendedColxnDTO colxn, BillCode billCode)
        {
            var actual = colxn.Actuals.For(billCode);
            var target = colxn.Targets.For(billCode);
            return new CollectorPerfCell(actual, target);
        }
    }
}
