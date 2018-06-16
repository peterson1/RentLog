using PropertyChanged;
using RentLog.Cashiering.SectionTabs.AmbulantCollections;
using RentLog.Cashiering.SectionTabs.IntendedCollections;
using RentLog.Cashiering.SectionTabs.NoOperations;
using RentLog.Cashiering.SectionTabs.Uncollecteds;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;

namespace RentLog.Cashiering.SectionTabs
{
    [AddINotifyPropertyChangedInterface]
    public class SectionTabVM
    {
        public SectionTabVM(SectionDTO sec, MainWindowVM main)
        {
            Section        = sec;
            Collector      = main.ColxnsDB.GetCollector(sec);
            IntendedColxns = new IntendedColxnsVM(Collector, sec, main);
            AmbulantColxns = new AmbulantColxnsVM(sec, main);
            NoOperations   = new NoOperationsVM  (sec, main);
            Uncollecteds   = new UncollectedsVM  (this, main);
        }


        public SectionDTO         Section          { get; }
        public CollectorDTO       Collector        { get; }
        public IntendedColxnsVM   IntendedColxns   { get; }
        public AmbulantColxnsVM   AmbulantColxns   { get; }
        public NoOperationsVM     NoOperations     { get; }
        public UncollectedsVM     Uncollecteds     { get; }

        public decimal SectionTotal => IntendedColxns.TotalSum
                                     + AmbulantColxns.TotalSum;

        internal void ReloadAll()
        {
            IntendedColxns.ReloadFromDB();
            AmbulantColxns.ReloadFromDB();
            NoOperations  .ReloadFromDB();
            Uncollecteds  .ReloadFromDB();
        }
    }
}
