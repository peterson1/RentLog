using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public class LeaseConverter1 : ConverterBase<ReportModels.Lease, LeaseDTO>
    {
        public override LeaseDTO Convert(ReportModels.Lease byf) => new LeaseDTO
        {
            ContractStart = byf.ContractStart,
            ContractEnd   = byf.ContractEnd,
        };
    }
}
