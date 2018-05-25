using PropertyChanged;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.LeasesCrud.LeasesList;
using RentLog.LeasesCrud.MainToolbar;

namespace RentLog.LeasesCrud
{
    [AddINotifyPropertyChangedInterface]
    internal class MainWindowVM : BrandedWindowBase
    {
        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            MainToolBar    = new MainToolbarVM(appArguments);
            ActiveLeases   = new ActiveLeasesVM(appArguments);
            InactiveLeases = new InactiveLeasesVM(appArguments);
            ActiveLeases.Crud.SaveCompleted += (s, e) => ClickRefresh();
            ActiveLeases.LeaseDeactivated   += (s, e) => ClickRefresh();
            ClickRefresh();
        }


        public MainToolbarVM     MainToolBar     { get; }
        public ActiveLeasesVM    ActiveLeases    { get; }
        public InactiveLeasesVM  InactiveLeases  { get; }


        protected override void OnRefreshClicked()
        {
            MainToolBar   .UpdateAll();
            ActiveLeases  .ReloadFromDB();
            InactiveLeases.ReloadFromDB();
        }
    }
}
