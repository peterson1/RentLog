using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.DomainLib45.SoaViewer.MainWindow;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.LeasesCrud.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class ActiveLeasesVM : IndirectFilteredListVMBase<LeaseBalanceRow, LeaseDTO, LeasesFilterVM, AppArguments>
    {
        public ActiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.ActiveLeases, appArguments, false)
        {
        }


        public string Caption { get; private set; }


        protected override void OnItemOpened(LeaseDTO e)
            => SoaViewer.Show(e, AppArgs);


        protected override List<LeaseDTO> QueryItems(ISimpleRepo<LeaseDTO> db)
        {
            var items = db.GetAll().OrderByDescending(_ => _.Id).ToList();
            Caption   = $"  Active Leases  ({items.Count:N0})  ";
            return items;
        }

        protected override LeaseBalanceRow CastToRow(LeaseDTO lse)
        {
            var date = AppArgs.Collections.LastPostedDate();
            var bill = AppArgs.Balances.GetBill(lse, date);
            return new LeaseBalanceRow(lse, bill);
        }
    }
}
