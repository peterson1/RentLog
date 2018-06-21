using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.DcdrTab.ReportSettings;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ChequeVouchers.DcdrTab.PassbookRows
{
    [AddINotifyPropertyChangedInterface]
    public class PassbookRowsVM : FilteredSavedListVMBase<PassbookRowDTO, PassbookRowsFilterVM, ITenantDBsDir>
    {
        private DateRangePickerVM _rnge;
        private ITenantDBsDir     _dir;


        public PassbookRowsVM(DateRangePickerVM dateRangePickerVM, ITenantDBsDir tenantDBsDir) : base(null, tenantDBsDir, false)
        {
            _dir  = tenantDBsDir;
            _rnge = dateRangePickerVM;
        }


        public PassbookRowDTO  LastRow      { get; private set; }


        protected override bool CanAddNewItem() => AppArgs.CanAddPassbookRow(false);

        protected override void AddNewItem()
        {
            var crud = new PassbookRowCrudVM(Repo, AppArgs);
            crud.SaveCompleted += (s, e) =>
            {
                Repo.RecomputeBalancesFrom(e.TransactionDate);
                ReloadFromDB();
            };
            crud.EncodeNewDraftCmd.ExecuteIfItCan();
        }


        protected override void OnItemsReplaced()
        {
            LastRow = ItemsList.LastOrDefault();
        }


        protected override List<PassbookRowDTO> QueryItems(ISimpleRepo<PassbookRowDTO> db)
            => Repo.RowsFor(_rnge.Start, _rnge.End);


        private IPassbookRowsRepo Repo => _dir.Passbooks.GetRepo(_dir.CurrentBankAcct.Id);
    }
}
