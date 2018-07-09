using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.ReportRows
{
    public class CollectorPerformanceRow
    {
        public CollectorDTO  Collector     { get; }
        public string        SectionsText  { get; }
    }
}
