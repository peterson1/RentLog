using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.ChequeVouchers.DcdrTab.ReportSettings;
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
        private readonly DateRangePickerVM _rnge;
        private readonly ITenantDBsDir     _dir;


        public PassbookRowsVM(DateRangePickerVM dateRangePickerVM, ITenantDBsDir tenantDBsDir) : base(null, tenantDBsDir, false)
        {
            _dir  = tenantDBsDir;
            _rnge = dateRangePickerVM;
            ItemsList.ItemsReplaced += (s, e)
                => LastRow = ItemsList.LastOrDefault();
        }


        public PassbookRowDTO LastRow { get; private set; }


        //not working
        //protected override void OnItemsReplaced()
        //{
        //    LastBalance = ItemsList.LastOrDefault()?.RunningBalance;
        //}


        protected override List<PassbookRowDTO> QueryItems(ISimpleRepo<PassbookRowDTO> db)
            => Repo.RowsFor(_rnge.Start, _rnge.End);


        private IPassbookRowsRepo Repo => _dir.Passbooks.GetRepo(_dir.CurrentBankAcct.Id);
    }
}
