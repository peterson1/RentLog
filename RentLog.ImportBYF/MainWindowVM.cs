﻿using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.ImportBYF.Converters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.ImportBYF
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Import BYF";

        private Dictionary<int, Func<ComparisonsListBase>> _enlisteds = new Dictionary<int, Func<ComparisonsListBase>>();


        public MainWindowVM(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
        }


        public UIList<string>       ListNames        { get; } = new UIList<string>();
        public int                  PickedListIndex  { get; set; } = -1;
        public ComparisonsListBase  PickedList       { get; private set; }
        public string               PickedListName   => ListNames[PickedListIndex];


        protected override void OnWindowLoaded()
        {
            if (PickedListIndex == -1)
                PickedListIndex = 0;
        }


        public void OnPickedListIndexChanged()
        {
            PickedList = null;
            PickedList = _enlisteds[PickedListIndex]();
            ClickRefresh();
        }


        protected override async Task OnRefreshClickedAsync()
        {
            if (PickedList == null) return;
            StartBeingBusy($"Loading {PickedListName} ...");
            await Task.Delay(1);
            await Task.Run(() => PickedList.ReloadList(AppArgs));
            StopBeingBusy();
        }


        private LeaseDTO CastBYF(ReportModels.Lease byf) => new LeaseDTO
        {
            ContractStart = byf.ContractStart,
            ContractEnd = byf.ContractEnd,
        };
    }
}
