using PropertyChanged;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.LeasesCrud.LeasesList;
using RentLog.LeasesCrud.MainToolbar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RentLog.LeasesCrud
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Leases";


        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            MainToolBar    = new MainToolbarVM(this);
            ActiveLeases   = new ActiveLeasesVM(appArguments);
            InactiveLeases = new InactiveLeasesVM(appArguments);
            ActiveLeases.Crud.SaveCompleted += (s, e) => ClickRefresh();
            ActiveLeases.LeaseDeactivated   += (s, e) => ClickRefresh();
            ClickRefresh();
        }


        public MainToolbarVM     MainToolBar     { get; }
        public ActiveLeasesVM    ActiveLeases    { get; }
        public InactiveLeasesVM  InactiveLeases  { get; }

        public IEnumerable<LeaseBalanceRow>  CurrentRows     { get; set; }
        public bool                          CurrentIsActive { get; set; }
        public DataGrid                      CurrentTable    { get; set; }


        protected override void OnRefreshClicked()
        {
            Parallel.Invoke
            (
                () => MainToolBar.UpdateAll(),
                () => ActiveLeases.ReloadFromDB(),
                () => InactiveLeases.ReloadFromDB()
            );
        }
    }
}
