﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.DomainLib45.SoaViewer.MainWindow;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.LeasesCrud.LeasesList
{
    public class ActiveLeasesVM : IndirectFilteredListVMBase<ActiveLeaseRow, LeaseDTO, ActiveLeasesFilterVM, AppArguments>
    {
        public ActiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.ActiveLeases, appArguments, true)
        {
        }


        protected override void OnItemOpened(LeaseDTO e)
            => SoaViewer.Show(e, AppArgs);


        protected override List<LeaseDTO> QueryItems(ISimpleRepo<LeaseDTO> db)
            => db.GetAll().OrderByDescending(_ => _.Id).ToList();


        protected override ActiveLeaseRow CastToRow(LeaseDTO lse)
        {
            var row           = new ActiveLeaseRow(lse);
            var date          = AppArgs.Collections.LastPostedDate();
            var bill          = AppArgs.Balances.GetBill(lse, date);
            row.RentBalance   = bill.For(BillCode.Rent  ).ClosingBalance;
            row.RightsBalance = bill.For(BillCode.Rights).ClosingBalance;
            return row;
        }
    }
}
