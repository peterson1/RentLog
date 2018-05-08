using PropertyChanged;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.LeasesCrud.LeasesList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.LeasesCrud
{
    [AddINotifyPropertyChangedInterface]
    internal class MainWindowVM : BrandedWindowBase
    {
        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
            ActiveLeases = new ActiveLeasesVM(appArguments);
        }



        public ActiveLeasesVM    ActiveLeases    { get; }
        public InactiveLeasesVM  InactiveLeases  { get; }
    }
}
