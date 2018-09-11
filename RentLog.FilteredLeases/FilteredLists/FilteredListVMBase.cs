using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.FilteredLeases.FilteredLists
{
    public class FilteredListVMBase : IndirectFilteredListVMBase<LeaseBalanceRow, LeaseDTO, CommonTextFilterVM, ITenantDBsDir>
    {
        public FilteredListVMBase(ISimpleRepo<LeaseDTO> simpleRepo, ITenantDBsDir appArguments, bool doReload = true) 
            : base(simpleRepo, appArguments, doReload)
        {
        }


        protected override LeaseBalanceRow CastToRow(LeaseDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
