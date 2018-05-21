using System.Threading.Tasks;
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
            MainToolBar  = new MainToolbarVM(appArguments);
            ActiveLeases = new ActiveLeasesVM(appArguments);
            ClickRefresh();
        }


        public MainToolbarVM     MainToolBar     { get; }
        public ActiveLeasesVM    ActiveLeases    { get; }
        public InactiveLeasesVM  InactiveLeases  { get; }


        protected override void OnRefreshClicked()
        {
            MainToolBar.UpdateAll();
            ActiveLeases.ReloadFromDB();
        }


        //protected override async Task OnRefreshClickedAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        MainToolBar.UpdateAll();
        //        ActiveLeases.ReloadFromDB();
        //    });
        //}
    }
}
