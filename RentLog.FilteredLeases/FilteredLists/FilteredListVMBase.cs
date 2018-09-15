using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.ReportRows;
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
        private MainWindowVM _main;

        public event EventHandler PrintRequested = delegate { };


        public FilteredListVMBase() : this(null, null)
        {
        }

        public FilteredListVMBase(MainWindowVM mainWindowVM, ITenantDBsDir dir) 
            : base(null, dir, false)
        {
            _main = mainWindowVM;
            FillSectionsList();
        }


        public UIList<SectionDTO>  Sections       { get; } = new UIList<SectionDTO>();
        public SectionDTO          PickedSection  { get; set; }

        protected abstract List<LeaseDTO> GetLeases(MarketStateDB mkt, int sectionId);


        public void OnPickedSectionChanged() => ReloadFromDB();


        public override async void ReloadFromDB()
        {
            _main.StartBeingBusy($"Loading “{_main.PickedFilterName}”...");
            await Task.Delay(1);
            await Task.Run(() => base.ReloadFromDB());
            _main.StopBeingBusy();
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
