using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer;
using RentLog.ChequeVouchers.DcdrTab.ReportSettings;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System;
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


        public PassbookRowDTO   LastRow   { get; private set; }


        protected override bool CanAddNewItem () => AppArgs.CanAddPassbookRow(false);
        protected override bool CanDeletetRecord(PassbookRowDTO rec)
        {
            if (rec.DocRefType != typeof(PassbookRowDTO).FullName) return false;
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
                case "RentLog.DomainLib11.DTOs.PassbookRowDTO":
                    if (AppArgs.CanEditPassbookRow(true))
                        CreateCrud().EditCurrentRecord(rec);
                    break;

                case "PassbookTally.DomainLib.DTOs.RequestedChequeDTO":
                    ChequeVoucherViewerVM.Show(rec.As<ChequeVoucherDTO>(), AppArgs);
                    break;

                default: break;
            }
        }


        protected override void OnItemsReplaced()
            => LastRow = ItemsList.LastOrDefault();


        protected override List<PassbookRowDTO> QueryItems(ISimpleRepo<PassbookRowDTO> db)
            => Repo.RowsFor(_rnge.Start, _rnge.End);


        private IPassbookRowsRepo Repo 
            => _dir.Passbooks.GetRepo(_dir.CurrentBankAcct.Id);


        protected override void DeleteRecord(ISimpleRepo<PassbookRowDTO> db, PassbookRowDTO dto) 
            => Repo.Delete(dto);
    }
}
