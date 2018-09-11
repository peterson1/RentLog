using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.FilteredLeases.FilteredLists;
using System;

namespace RentLog.FilteredLeases
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Filtered Leases";


        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
        }


        public UIList<string> FilterNames { get; } = new UIList<string>
        {
            "All Active Leases",
            "All Inactive Leases",
            "With Backrents or Overdue Rights",
            "Leases Nearing Rights Expiry"
        };


        public int                 PickedFilterIndex  { get; set; }
        public FilteredListVMBase  PickedList         { get; private set; }


        public void OnPickedFilterIndexChanged()
        {
            //Alert.Show($"[{PickedFilterIndex}] {FilterNames[PickedFilterIndex]}");
            PickedList = null;
            PickedList = CreatePickedList();
        }


        private FilteredListVMBase CreatePickedList()
        {
            throw new NotImplementedException();
        }
    }
}
