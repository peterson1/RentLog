﻿using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.StringTools;
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
            : base(GetRepo(sectionTabVM, main), main)
        {
            _tab         = sectionTabVM;
            CanAddRows   = false;
            TotalVisible = false;
            Filter.TextFilterChanged += (s, e) => ApplyTextFilters();
        }


        public UncollectedsFilterVM Filter { get; } = new UncollectedsFilterVM();


        protected override void OnItemOpened(UncollectedLeaseDTO e) 
            => _tab.EncodeNewIntendedColxn(e);


        public override void ReloadFromDB()
        {
            _queried = GetPostProcessedResult().ToList();
            ApplyTextFilters();
        }


        protected override void DeleteRecord(ISimpleRepo<UncollectedLeaseDTO> db, UncollectedLeaseDTO dto)
            => Main.ColxnsDB.NoOperations[_tab.Section.Id].Insert(dto);


        protected override void DeleteRecords(ISimpleRepo<UncollectedLeaseDTO> db, List<UncollectedLeaseDTO> items)
            => Main.ColxnsDB.NoOperations[_tab.Section.Id].Insert(items, true);


        private void ApplyTextFilters()
        {
            var list = _queried.ToList();
            Filter.RemoveNonMatches(ref list);
            UIThread.Run(() => ItemsList.SetItems(list));
        }


        private List<UncollectedLeaseDTO> GetUpdatedUncollecteds(ISimpleRepo<UncollectedLeaseDTO> db)
        {
            //return (db as IUncollectedsRepo).InferUncollecteds(
            //                   _tab.IntendedColxns.ItemsList,
            //                   _tab.NoOperations.ItemsList);
            var repo    = db as IUncollectedsRepo;
            var intents = _tab.IntendedColxns.ItemsList;
            var no_ops  = _tab.NoOperations.ItemsList;
            try
            {
                return repo.InferUncollecteds(intents, no_ops);
            }
            catch (LockedFileException ex)
            {
                Alert.ShowModal("Access Conflict caused the file to be locked.",
                                "Please restart your PC, login to your Windows account," 
                                + L.f + "then wait for 5 mins. before relaunching MSA."
                                + L.F + ex.Info());
                App.Current.Shutdown();
                return null;
            }
        }


        protected override List<UncollectedLeaseDTO> QueryItems(ISimpleRepo<UncollectedLeaseDTO> db)
            => Main.CanEncode ? GetUpdatedUncollecteds(db)
                              : base.QueryItems(db);


        private static ISimpleRepo<UncollectedLeaseDTO> GetRepo(SectionTabVM tab, MainWindowVM main)
            => main.ColxnsDB.Uncollecteds[tab.Section.Id];


        protected override void OnTotalSumChanged() { }
        protected override bool CanDeleteRecord(UncollectedLeaseDTO rec) => Main.CanEncode;
        protected override string ListTitle => "Uncollecteds";
        protected override Func<UncollectedLeaseDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        //protected override Func<UncollectedLeaseDTO, decimal>  SummedAmount => _ => _.Targets.Total;
    }
}
