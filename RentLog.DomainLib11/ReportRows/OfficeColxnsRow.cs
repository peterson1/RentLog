using System;
using System.Collections.Generic;
using System.Text;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib11.ReportRows
{
    public class OfficeColxnsRow : SectionColxnsRow
    {
        public OfficeColxnsRow(DateTime date, ITenantDBsDir dir) : base(SectionDTO.Named("Office"), date, dir)
        {
        }
    }
}
