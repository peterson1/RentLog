using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerfCell
    {
        public CollectorPerfCell(decimal? actual, decimal? target)
        {
            Actual = actual;
            Target = target;
        }

        public decimal   Shortage  { get; }
        public decimal   Overage   { get; }
        public decimal?  Target    { get; }
        public decimal?  Actual    { get; }
        public decimal   NoExcess  { get; }
        public bool  IsOver    => Overage != 0;
        public bool  IsShort   => Shortage != 0;
    }
}
