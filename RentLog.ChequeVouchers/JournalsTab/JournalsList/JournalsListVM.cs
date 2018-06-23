using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsList
{
    public class JournalsListVM : FilteredSavedListVMBase<JournalVoucherDTO, JournalsFilterVM, ITenantDBsDir>
    {
        public JournalsListVM(ISimpleRepo<JournalVoucherDTO> simpleRepo, ITenantDBsDir appArguments, bool doReload = true) : base(simpleRepo, appArguments, doReload)
        {
        }
    }
}
