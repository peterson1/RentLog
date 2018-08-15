using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ChequeVouchers.AllChequeVouchers
{
    [AddINotifyPropertyChangedInterface]
    public class MainListVM : FilteredSavedListVMBase<FundRequestDTO, FilterVM, ITenantDBsDir>
    {
        public MainListVM(ISimpleRepo<FundRequestDTO> simpleRepo, ITenantDBsDir appArguments, bool doReload = true) : base(simpleRepo, appArguments, doReload)
        {
        }


        protected override List<FundRequestDTO> QueryItems(ISimpleRepo<FundRequestDTO> db) 
            => base.QueryItems(db).Where(_ => _.BankAccountId == AppArgs.CurrentBankAcct.Id)
                                  .OrderByDescending(_ => _.SerialNum)
                                  .ToList();
    }
}
