using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer;
using RentLog.ChequeVouchers.MainToolbar;
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
        private const string PASSBOOKROW_TYPE = "RentLog.DomainLib11.DTOs.PassbookRowDTO";


        public PassbookRowsVM(MainWindowVM main) : base(null, main.AppArgs, false)
        {
            _rnge = main.DateRange;
        }


        public PassbookRowDTO   LastRow   { get; private set; }


        protected override bool CanAddNewItem () => AppArgs.CanAddPassbookRow(false);
        protected override bool CanDeleteRecord(PassbookRowDTO rec)
        {
            if (rec.DocRefType != PASSBOOKROW_TYPE)
                return AppArgs.CanDeleteSystemGeneratedPassbookRow(true);

            return AppArgs.CanDeletePassbookRow(true);
        }


        private PassbookRowCrudVM CreateCrud()
        {
            var crud = new PassbookRowCrudVM(Repo, AppArgs);
            crud.SaveCompleted += (s, e) =>
            {
                Repo.RecomputeBalancesFrom(e.TransactionDate);
                ReloadFromDB();
            };
            return crud;
        }


        protected override void AddNewItem() 
            => CreateCrud().EncodeNewDraftCmd.ExecuteIfItCan();


        protected override void LoadRecordForEditing(PassbookRowDTO rec)
        {
            switch (rec.DocRefType)
            {
                case PASSBOOKROW_TYPE:
                    if (AppArgs.CanEditPassbookRow(true))
                        CreateCrud().EditCurrentRecord(rec);
                    break;

                case "PassbookTally.DomainLib.DTOs.RequestedChequeDTO":
                case "RentLog.DomainLib11.DTOs.ChequeVoucherDTO":
                    var vm = ChequeVoucherViewerVM.Show(rec.As<ChequeVoucherDTO>(), rec.TransactionDate, AppArgs);
                    vm.ClearedDateUpdated += (s, e) => ReloadFromDB();
                    break;

                default: break;
            }
        }


        protected override void OnItemsReplaced()
            => LastRow = ItemsList.LastOrDefault();


        protected override List<PassbookRowDTO> QueryItems(ISimpleRepo<PassbookRowDTO> db)
            => Repo.RowsFor(_rnge.Start, _rnge.End);


        private IPassbookRowsRepo Repo 
            => AppArgs.Passbooks.GetRepo(AppArgs.CurrentBankAcct.Id);


        protected override void DeleteRecord(ISimpleRepo<PassbookRowDTO> db, PassbookRowDTO dto) 
            => Repo.Delete(dto);
    }
}
