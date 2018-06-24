﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.ChequeVouchers.CommonControls.ChequeVoucherViewer;
using RentLog.ChequeVouchers.JournalsTab.JournalsCrud;
using RentLog.ChequeVouchers.MainToolbar;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.PassbookRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsList
{
    public class JournalsListVM : FilteredSavedListVMBase<JournalVoucherDTO, JournalsFilterVM, ITenantDBsDir>
    {
        private DateRangePickerVM _rnge;


        public JournalsListVM(MainWindowVM main) : base(null, main.AppArgs, false)
        {
            Crud  = new JournalsCrudVM(AppArgs);
            _rnge = main.DateRange;
        }


        public JournalsCrudVM  Crud  { get; }


        protected override List<JournalVoucherDTO> QueryItems(ISimpleRepo<JournalVoucherDTO> db)
            => AppArgs.Journals.List(_rnge.Start, _rnge.End);


        public    override bool CanEditRecord        (JournalVoucherDTO rec) => AppArgs.CanEditJournalVoucher   (true);
        protected override bool CanDeleteRecord      (JournalVoucherDTO rec) => AppArgs.CanDeleteJournalVoucher (true);
        protected override void LoadRecordForEditing (JournalVoucherDTO rec) => Crud   .EditCurrentRecord       (rec);
    }
}
