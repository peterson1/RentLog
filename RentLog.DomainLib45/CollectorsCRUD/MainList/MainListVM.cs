using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.DomainLib45.CollectorsCRUD.MainList
{
    [AddINotifyPropertyChangedInterface]
    public class MainListVM : FilteredSavedListVMBase<CollectorDTO, FilterVM, ITenantDBsDir>
    {
        public MainListVM(ISimpleRepo<CollectorDTO> simpleRepo, ITenantDBsDir appArguments, bool doReload = true) : base(simpleRepo, appArguments, doReload)
        {
        }
    }
}
