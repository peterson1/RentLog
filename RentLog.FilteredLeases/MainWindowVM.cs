using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.FilteredLeases.FilteredLists;
using RentLog.FilteredLeases.FilteredLists.AllActiveLeases;
using RentLog.FilteredLeases.FilteredLists.AllInactiveLeases;
using RentLog.FilteredLeases.FilteredLists.WithBackRentsOrRights;

namespace RentLog.FilteredLeases
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        private Dictionary<int, Func<ITenantDBsDir, FilteredListVMBase>> _enlisteds = new Dictionary<int, Func<ITenantDBsDir, FilteredListVMBase>>();

        public override string SubAppName => "Filtered Leases";


        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            Enlist("All Active Leases", _ => new AllActiveLeasesVM(_));
            Enlist("All Inactive Leases", _ => new AllInactiveLeasesVM(_));
            Enlist("With Backrents or Overdue Rights", _ => new WithBackRentsOrRightsVM(_));

            PickedFilterIndex = 0;
        }


        public UIList<string> FilterNames { get; } = new UIList<string>();
        //{
        //    "All Active Leases",
        //    "All Inactive Leases",
        //    "With Backrents or Overdue Rights",
        //    "Leases Nearing Rights Expiry"
        //};


        public int                 PickedFilterIndex  { get; set; } = -1;
        public FilteredListVMBase  PickedList         { get; private set; }


        private void Enlist(string label, Func<ITenantDBsDir, FilteredListVMBase> constructor)
        {
            _enlisteds.Add(_enlisteds.Count, constructor);
            FilterNames.Add(label);
        }


        public async void OnPickedFilterIndexChanged()
        {
            await Task.Delay(1);
            StartBeingBusy($"Loading “{FilterNames[PickedFilterIndex]}”...");
            PickedList = null;
            PickedList = _enlisteds[PickedFilterIndex].Invoke(AppArgs);
            await Task.Run(() => PickedList.ReloadFromDB());
            StopBeingBusy();
        }
    }
}
