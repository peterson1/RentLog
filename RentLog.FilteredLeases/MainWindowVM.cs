using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib11.EventHandlerTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.FilteredLeases.FilteredLists;
using RentLog.FilteredLeases.FilteredLists.AllActiveLeases;
using RentLog.FilteredLeases.FilteredLists.AllInactiveLeases;
using RentLog.FilteredLeases.FilteredLists.WithBackRentsOrRights;
using System;
using System.Collections.Generic;
using System.Linq;
using RentLog.FilteredLeases.FilteredLists.FullTenantDetails;
using RentLog.FilteredLeases.MainToolbar;
using RentLog.FilteredLeases.FilteredLists.UpcomingBirthdays;

namespace RentLog.FilteredLeases
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        private Dictionary<int, Func<ITenantDBsDir, FilteredListVMBase>> _enlisteds = new Dictionary<int, Func<ITenantDBsDir, FilteredListVMBase>>();

        public event EventHandler ToExcelRequested = delegate { };
        public event EventHandler PickedListLoaded = delegate { };

        public override string SubAppName => "Filtered Leases";


        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            AdHocJobsCmds    = new AdHocJobCmdsVM(this);
            ExportToExcelCmd = R2Command.Relay(() => ToExcelRequested.Raise(), null, "Export to Excel");

            Enlist("All Active Leases", _ => new AllActiveLeasesVM(this, _));
            Enlist("All Terminated Leases", _ => new AllInactiveLeasesVM(this, _));
            Enlist("With Backrents or Overdue Rights", _ => new WithBackRentsOrRightsVM(this, _));
            Enlist("Full Tenant Details", _ => new FullTenantDetailsVM(this, _));
            Enlist("Upcoming Birthdays", _ => new UpcomingBirthdaysVM(this, _));
            //todo: "Leases Nearing Rights Expiry"
        }


        public AdHocJobCmdsVM      AdHocJobsCmds      { get; }
        public IR2Command          ExportToExcelCmd   { get; }
        public UIList<string>      FilterNames        { get; } = new UIList<string>();
        public int                 PickedFilterIndex  { get; set; } = -1;
        public FilteredListVMBase  PickedList         { get; private set; }
        public string              PickedFilterName   => FilterNames[PickedFilterIndex];
        public string              PickedSectionName  => PickedList?.PickedSection?.Name;
        public string              SectionAndFilter   => $"{PickedSectionName}  :  {PickedFilterName} ({PickedList?.Rows?.Count})";


        private void Enlist(string label, Func<ITenantDBsDir, FilteredListVMBase> constructor)
        {
            _enlisteds.Add(_enlisteds.Count, constructor);
            FilterNames.Add(label);
        }


        protected override void OnWindowLoaded()
        {
            if (PickedFilterIndex == -1)
                PickedFilterIndex = 0;
        }


        public void OnPickedFilterIndexChanged()
        {
            PickedList = null;
            PickedList = _enlisteds[PickedFilterIndex].Invoke(AppArgs);
            PickedList.PickedSection = PickedList.Sections.FirstOrDefault();
        }


        public void RaisePickedListLoaded() => PickedListLoaded.Raise();
    }
}
