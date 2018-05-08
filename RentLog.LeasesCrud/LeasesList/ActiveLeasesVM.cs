using CommonTools.Lib45.BaseViewModels;
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


        protected override ActiveLeaseRow CastToRow(LeaseDTO lse)
        {
            var row           = new ActiveLeaseRow(lse);
            var bill          = AppArgs.Balances.GetLatest(lse);
            row.RentBalance   = bill.For(BillCode.Rent  ).OpeningBalance;
            row.RightsBalance = bill.For(BillCode.Rights).OpeningBalance;
            return row;
        }
    }
}
