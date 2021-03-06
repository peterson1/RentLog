﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.EventHandlerTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.MathTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.FilteredLeases.FilteredLists
{
    public abstract class FilteredListVMBase : IndirectFilteredListVMBase<LeaseBalanceRow, LeaseDTO, CommonTextFilterVM, ITenantDBsDir>
    {
        private ConcurrentDictionary<int, DailyBillDTO> _bills = new ConcurrentDictionary<int, DailyBillDTO>();

        public       EventHandler _printRequested;
        public event EventHandler  PrintRequested
        {
            add    { _printRequested = null; _printRequested += value; }
            remove { _printRequested -= value; }
        }


        public FilteredListVMBase(MainWindowVM mainWindowVM, ITenantDBsDir dir) 
            : base(null, dir, false)
        {
            Main = mainWindowVM;
            EncodeNewDraftCmd = CreateEncodeNewDraftCmd(dir);
            PrintCmd = R2Command.Relay(() => _printRequested?.Raise(), null, "Print");
            FillSectionsList();
        }


        public MainWindowVM        Main               { get; }
        public IR2Command          EncodeNewDraftCmd  { get; }
        public UIList<SectionDTO>  Sections           { get; } = new UIList<SectionDTO>();
        public SectionDTO          PickedSection      { get; set; }
        public bool                IsPrinting         { get; set; }
        public IR2Command          PrintCmd           { get; }


        protected abstract List<LeaseDTO> GetLeases(MarketStateDbBase mkt, int sectionId);


        public             void OnPickedSectionChanged ()             => ReloadFromDB();
        protected override void OnItemOpened           (LeaseDTO lse) => SoaViewer.Show(lse, AppArgs);


        public virtual string TopLeftText => Main?.SectionAndFilter;
        protected virtual IR2Command CreateEncodeNewDraftCmd(ITenantDBsDir dir) => null;

        public virtual Action DoAfterSave => () => this.ClickRefresh();


        public virtual string TopRightText
        {
            get
            {
                var rentBal   = Rows.Sum(_ => _.Rent.ZeroIfNullOrNegative());
                var rightsBal = Rows.Sum(_ => _.Rights.ZeroIfNullOrNegative());
                return $"Total Rent Balance :  {rentBal:N2}" + L.f
                     + $"Total Rights Balance :  {rightsBal:N2}";
            }
        }


        public override async void ReloadFromDB()
        {
            Main.StartBeingBusy($"Loading “{Main.PickedFilterName}”...");
            await Task.Delay(1);
            await Task.Run(() => base.ReloadFromDB());
            Main.RaisePickedListLoaded();
            Main.StopBeingBusy();
        }


        protected override List<LeaseDTO> QueryItems(ISimpleRepo<LeaseDTO> db)
        {
            var lses = GetLeases(AppArgs.MarketState, PickedSection?.Id ?? 0);
            FillBillsDictionary(lses);
            return lses;
        }


        private void FillBillsDictionary(List<LeaseDTO> lses)
        {
            var date = AppArgs.Collections.LastPostedDate();
            var jobs = lses.Select(_ => GetBillQueryJob(_, date));
            Parallel.Invoke(jobs.ToArray());
        }


        private Action GetBillQueryJob(LeaseDTO lse, DateTime date) => () =>
        {
            DailyBillDTO bill = null;
            try
            {
                bill = GetBill(lse, date);
            }
            catch (Exception ex)
            {
                Alert.Show(ex, $"Getting bill for [{lse.Id}] “{lse}”");
            }
            _bills.TryAdd(lse.Id, bill);
        };


        private DailyBillDTO GetBill(LeaseDTO lse, DateTime date)
            => lse is InactiveLeaseDTO
             ? AppArgs.Balances.GetRepo(lse).Latest()
             : AppArgs.Balances.GetBill(lse, date);


        protected override LeaseBalanceRow CastToRow(LeaseDTO lse)
        {
            var found = _bills.TryGetValue(lse.Id, out DailyBillDTO bill);
            return new LeaseBalanceRow(lse, found ? bill : null);
        }


        private void FillSectionsList()
        {
            Sections.Add(SectionDTO.Named("All Sections"));
            foreach (var sec in AppArgs.MarketState.Sections.GetAll())
                Sections.Add(sec);
        }
    }
}
