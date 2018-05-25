using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.LeasesCrud.LeasesList
{
    [AddINotifyPropertyChangedInterface]
    public class InactiveLeasesVM : IndirectFilteredListVMBase<LeaseBalanceRow, InactiveLeaseDTO, LeasesFilterVM, AppArguments>
    {
        public InactiveLeasesVM(AppArguments appArguments) : base(appArguments.MarketState.InactiveLeases, appArguments, false)
        {
        }


        public string Caption { get; private set; }


        protected override void OnItemOpened(InactiveLeaseDTO e)
            => SoaViewer.Show(e, AppArgs);


        protected override List<InactiveLeaseDTO> QueryItems(ISimpleRepo<InactiveLeaseDTO> db)
        {
            var items = db.GetAll().OrderByDescending(_ => _.DeactivatedDate).ToList();
            Caption   = $"  Inactive Leases  ({items.Count:N0})  ";
            return items;
        }

        protected override LeaseBalanceRow CastToRow(InactiveLeaseDTO lse)
        {
            var repo = AppArgs.Balances.GetRepo(lse);
            var bill = repo.Latest();
            return new LeaseBalanceRow(lse, bill);
        }
    }
}
