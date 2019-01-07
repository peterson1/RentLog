using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.CollectorsCRUD.MainEditor;

namespace RentLog.DomainLib45.CollectorsCRUD.MainList
{
    [AddINotifyPropertyChangedInterface]
    public class MainListVM : FilteredSavedListVMBase<CollectorDTO, FilterVM, ITenantDBsDir>
    {
        public MainListVM(ISimpleRepo<CollectorDTO> simpleRepo, ITenantDBsDir appArguments, bool doReload = true) : base(simpleRepo, appArguments, doReload)
        {
        }


        protected override void LoadRecordForEditing(CollectorDTO rec)
        {
            var crud = new MainEditorVM(AppArgs.MarketState.Collectors, AppArgs);
            crud.SaveCompleted += (s, e) => ClickRefresh();
            crud.EditCurrentRecord(rec);
        }


        protected override List<CollectorDTO> QueryItems(ISimpleRepo<CollectorDTO> db)
            => db.GetAll().OrderBy(_ => _.Id).ToList();
    }
}
