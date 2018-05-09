using System.Collections.Generic;
using System.Linq;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;

namespace RentLog.LeasesCrud.LeasesList
{
    public class ActiveLeasesVM : IndirectFilteredListVMBase<ActiveLeaseRow, LeaseDTO, ActiveLeasesFilterVM, AppArguments>
    {
        public ActiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.ActiveLeases, appArguments, true)
        {
        }


        protected override void OnItemOpened(LeaseDTO e)
        {
            Alert.Show(e.ToString());
        }


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
