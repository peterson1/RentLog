using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    [AddINotifyPropertyChangedInterface]
    public class UncollectedsVM : EncoderListVMBase<UncollectedLeaseDTO>
    {
        private SectionTabVM _tab;
        private List<UncollectedLeaseDTO> _queried;


        public UncollectedsVM(SectionTabVM sectionTabVM, MainWindowVM main) 
            : base(GetRepo(sectionTabVM, main), main, false)
        {
            _tab         = sectionTabVM;
            CanAddRows   = false;
            TotalVisible = false;
            Filter.TextFilterChanged += (s, e) => ApplyTextFilters();
        }


        public UncollectedsFilterVM Filter { get; } = new UncollectedsFilterVM();


        public override void ReloadFromDB()
        {
            _queried = GetPostProcessedResult().ToList();
            ApplyTextFilters();
        }


        private void ApplyTextFilters()
        {
            var list = _queried.ToList();
            Filter.RemoveNonMatches(ref list);
            UIThread.Run(() => ItemsList.SetItems(list));
        }


        private List<UncollectedLeaseDTO> GetUpdatedUncollecteds(ISimpleRepo<UncollectedLeaseDTO> db)
            => (db as IUncollectedsRepo).InferUncollecteds(
                    _tab.IntendedColxns.ItemsList,
                    _tab.NoOperations.ItemsList);


        protected override List<UncollectedLeaseDTO> QueryItems(ISimpleRepo<UncollectedLeaseDTO> db)
            => Main.CanEncode ? GetUpdatedUncollecteds(db)
                              : base.QueryItems(db);


        private static ISimpleRepo<UncollectedLeaseDTO> GetRepo(SectionTabVM tab, MainWindowVM main)
            => main.ColxnsDB.Uncollecteds[tab.Section.Id];


        protected override string ListTitle => "Uncollecteds";
        protected override Func<UncollectedLeaseDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        //protected override Func<UncollectedLeaseDTO, decimal>  SummedAmount => _ => _.Targets.Total;
    }
}
