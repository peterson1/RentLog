using PropertyChanged;
using RentLog.Cashiering.SectionTabs.AmbulantCollections;
using RentLog.Cashiering.SectionTabs.IntendedCollections;
using RentLog.Cashiering.SectionTabs.NoOperations;
using RentLog.Cashiering.SectionTabs.Uncollecteds;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.SectionTabs
{
    [AddINotifyPropertyChangedInterface]
    public class SectionTabVM
    {
        public SectionTabVM(SectionDTO sec, MainWindowVM main)
        {
            Main           = main;
            Section        = sec;
            Collector      = main.ColxnsDB.GetCollector(sec);
            IntendedColxns = new IntendedColxnsVM(this, Collector, sec, main);
            AmbulantColxns = new AmbulantColxnsVM(sec, main);
            NoOperations   = new NoOperationsVM  (sec, main);
            Uncollecteds   = new UncollectedsVM  (this, main);
        }


        public MainWindowVM         Main             { get; }
        public SectionDTO           Section          { get; }
        public CollectorDTO         Collector        { get; }
        public IntendedColxnsVM     IntendedColxns   { get; }
        public AmbulantColxnsVM     AmbulantColxns   { get; }
        public NoOperationsVM       NoOperations     { get; }
        public UncollectedsVM       Uncollecteds     { get; }


        public decimal SectionTotal => IntendedColxns.TotalSum
                                     + AmbulantColxns.TotalSum;

        internal void ReloadAll()
        {
            IntendedColxns.ReloadFromDB();
            AmbulantColxns.ReloadFromDB();
            NoOperations  .ReloadFromDB();
            Uncollecteds  .ReloadFromDB();
        }


        internal void EncodeNewIntendedColxn(UncollectedLeaseDTO dto)
        {
            if (!Main.CanEncode) return;
            var repo = Main.ColxnsDB.IntendedColxns[Section.Id];
            var vm   = new IntendedColxnCrudVM(dto, repo, Main.AppArgs);
            vm.EncodeNewDraftCmd.ExecuteIfItCan();
        }


        internal void EditIntendedColxn(IntendedColxnDTO dto)
        {
            if (!Main.CanEncode) return;
            var repo = Main.ColxnsDB.IntendedColxns[Section.Id];
            var vm = new IntendedColxnCrudVM(dto, repo, Main.AppArgs);
            vm.EditCurrentRecord(dto);
        }
    }
}
