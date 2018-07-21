using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerfSubRow
    {
        private IntendedColxnDTO _colxn;

        public CollectorPerfSubRow()
        {
        }


        public CollectorPerfSubRow(IntendedColxnDTO colxn, SectionDTO sectionDTO)
        {
            _colxn  = colxn;
            Section = sectionDTO;
            Rent    = CreateCell(colxn, BillCode.Rent);
            Rights  = CreateCell(colxn, BillCode.Rights);
        }


        public SectionDTO         Section  { get; }
        public CollectorPerfCell  Rent     { get; set; }
        public CollectorPerfCell  Rights   { get; set; }

        public LeaseDTO  Lease   => _colxn?.Lease;
        public StallDTO  Stall   => _colxn?.StallSnapshot;
        public string    Remarks => _colxn?.Remarks;


        private CollectorPerfCell CreateCell(IntendedColxnDTO colxn, BillCode billCode)
        {
            var actual = colxn.Actuals.For(billCode) ?? 0;
            var target = colxn.Targets.For(billCode) ?? 0;
            return new CollectorPerfCell(actual, target, billCode);
        }
    }
}
