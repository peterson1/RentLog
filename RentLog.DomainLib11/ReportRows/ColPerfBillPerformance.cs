using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class ColPerfBillPerformance
    {
        private ColPerfBillPerformance(BillCode billCode)
        {
            BillCode = billCode;
        }


        public BillCode  BillCode        { get; }
        public decimal   Target          { get; private set; }
        public decimal   Actual          { get; private set; }
        public decimal   NoExcess        { get; private set; }
        public decimal   PerfRate        { get; private set; }
        public int       UnderpaidCount  { get; private set; }
        public decimal   UnderpaidTotal  { get; private set; }
        public int       OverpaidCount   { get; private set; }
        public decimal   OverpaidTotal   { get; private set; }


        public static ColPerfBillPerformance New (BillCode billCode, IEnumerable<CollectorPerfCell> cells)
        {
            var bp = new ColPerfBillPerformance(billCode);
            bp.Target   = cells.Sum(_ => _.Target);
            bp.Actual   = cells.Sum(_ => _.Actual);
            bp.NoExcess = cells.Sum(_ => _.NoExcess);
            if (bp.Target != 0) bp.PerfRate = bp.NoExcess / bp.Target;

            bp.UnderpaidCount = cells.Count(_ => _.IsShort);
            bp.UnderpaidTotal = cells.Sum  (_ => _.Shortage);

            bp.OverpaidCount = cells.Count(_ => _.IsOver);
            bp.OverpaidTotal = cells.Sum  (_ => _.Overage);

            return bp;
        }
    }
}
