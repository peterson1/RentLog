﻿using PropertyChanged;
using RentLog.Cashiering.SectionTabs.AmbulantCollections;
using RentLog.Cashiering.SectionTabs.IntendedCollections;
using RentLog.Cashiering.SectionTabs.NoOperations;
using RentLog.Cashiering.SectionTabs.Uncollecteds;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using static RentLog.Cashiering.Properties.Settings;

namespace RentLog.Cashiering.SectionTabs
{
    [AddINotifyPropertyChangedInterface]
    public class SectionTabVM
    {
        public SectionTabVM(int tabIndex, SectionDTO sec, MainWindowVM main)
        {
            TabIndex       = tabIndex;
            Main           = main;
            Section        = sec;
            IntendedColxns = new IntendedColxnsVM(this, sec, main);
            AmbulantColxns = new AmbulantColxnsVM(sec, main);
            NoOperations   = new NoOperationsVM  (sec, main);
            Uncollecteds   = new UncollectedsVM  (this, main);
        }


        public int                  TabIndex         { get; }
        public MainWindowVM         Main             { get; }
        public SectionDTO           Section          { get; }
        public IntendedColxnsVM     IntendedColxns   { get; }
        public AmbulantColxnsVM     AmbulantColxns   { get; }
        public NoOperationsVM       NoOperations     { get; }
        public UncollectedsVM       Uncollecteds     { get; }

        public CollectorDTO Collector => IntendedColxns?.CurrentCollector;

        public bool    HasCollector => Collector != null;
        public bool    VacantsSaved => Main.ColxnsDB.HasVacantsTable(Section);
        public decimal SectionTotal => IntendedColxns.TotalSum
                                     + AmbulantColxns.TotalSum;

        internal void ReloadAll()
        {
            Parallel.Invoke(() => IntendedColxns.ReloadFromDB(),
                            () => AmbulantColxns.ReloadFromDB(),
                            () => NoOperations  .ReloadFromDB());
            Uncollecteds  .ReloadFromDB();
        }


        internal void EncodeNewIntendedColxn(UncollectedLeaseDTO dto)
        {
            if (!Main.CanEncode) return;
            var repo   = Main.ColxnsDB.IntendedColxns[Section.Id];
            var nextPR = (IntendedColxns.LastOrDefault()?.PRNumber ?? 0) + 1;
            var vm     = new IntendedColxnCrudVM(nextPR, dto, repo, Main.AppArgs);
            vm.EncodeNewDraftCmd.ExecuteIfItCan();
        }


        internal void EditIntendedColxn(IntendedColxnDTO dto)
        {
            if (!Main.CanEncode) return;
            var repo = Main.ColxnsDB.IntendedColxns[Section.Id];
            var vm = new IntendedColxnCrudVM(0, dto, repo, Main.AppArgs);
            vm.EditCurrentRecord(dto);
        }
    }
}
