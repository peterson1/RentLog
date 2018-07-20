using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.ReportRows
{
    public class ColPerfBillPerformance
    {
        public ColPerfBillPerformance(BillCode billCode, IEnumerable<CollectorPerfCell> cells)
        {
            BillCode = billCode;
            Target   = cells.Sum(_ => _.Target);
            NoExcess = cells.Sum(_ => _.NoExcess);
            if (Target != 0) PerfRate = NoExcess / Target;

            UnderpaidCount = cells.Count(_ => _.IsShort);
            UnderpaidTotal = cells.Sum(_ => _.Shortage);

            OverpaidCount = cells.Count(_ => _.IsOver);
            OverpaidTotal = cells.Sum(_ => _.Overage);
        }


        public BillCode  BillCode        { get; }
        public decimal   Target          { get; }
        public decimal   NoExcess        { get; }
        public decimal   PerfRate        { get; }
        public int       UnderpaidCount  { get; }
        public decimal   UnderpaidTotal  { get; }
        public int       OverpaidCount   { get; }
        public decimal   OverpaidTotal   { get; }
    }
}
