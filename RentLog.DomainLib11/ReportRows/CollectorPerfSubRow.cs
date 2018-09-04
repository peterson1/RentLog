using System;
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
            Rent    = CreateRentCell  (colxn);
            Rights  = CreateRightsCell(colxn);
        }


        public SectionDTO         Section  { get; }
        public CollectorPerfCell  Rent     { get; set; }
        public CollectorPerfCell  Rights   { get; set; }

        public LeaseDTO  Lease   => _colxn?.Lease;
        public StallDTO  Stall   => _colxn?.StallSnapshot;
        public string    Remarks => _colxn?.Remarks;


        private CollectorPerfCell CreateRentCell(IntendedColxnDTO colxn)
        {
            var billCode = BillCode.Rent;
            var actual   = colxn.Actuals.For(billCode) ?? 0;
            var target   = colxn.Targets.For(billCode) ?? 0;
            return new CollectorPerfCell(actual, target, billCode);
        }


        private CollectorPerfCell CreateRightsCell(IntendedColxnDTO colxn)
        {
            var billCode  = BillCode.Rights;
            var actual    = colxn.Actuals.For(billCode) ?? 0;
            var rightsBal = colxn.Targets.For(billCode);
            var target    = GetRightsTarget(rightsBal, colxn.Timestamp, colxn.Lease);
            return new CollectorPerfCell(actual, target, billCode);
        }


        public static decimal GetRightsTarget(decimal? rightsBalance, DateTime colxnDate, LeaseDTO lease)
        {
            var bal = rightsBalance ?? 0;
            if (bal <= 0) return 0;
            var daysLeft = (lease.RightsDueDate - colxnDate).TotalDays;
            if (daysLeft <= 0) return bal;
            return Math.Round(bal / (decimal)daysLeft);
        }
    }
}
