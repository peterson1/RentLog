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
        public SectionTabVM(SectionDTO sec, ICollectionsDB db, ITenantDBsDir tenantDBsDir)
        {
            Section        = sec;
            Collector      = db.GetCollector(sec);
            IntendedColxns = new IntendedColxnsVM(db.IntendedColxns[sec.Id], tenantDBsDir, this);
            AmbulantColxns = new AmbulantColxnsVM(db.AmbulantColxns[sec.Id], tenantDBsDir);
            Uncollecteds   = new UncollectedsVM  (db.Uncollecteds  [sec.Id], tenantDBsDir);
            NoOperations   = new NoOperationsVM  (db.NoOperations  [sec.Id], tenantDBsDir);
        }


        public SectionDTO         Section          { get; }
        public CollectorDTO       Collector        { get; }
        public IntendedColxnsVM   IntendedColxns   { get; }
        public AmbulantColxnsVM   AmbulantColxns   { get; }
        public UncollectedsVM     Uncollecteds     { get; }
        public NoOperationsVM     NoOperations     { get; }

        public decimal SectionTotal => IntendedColxns.TotalSum
                                     + AmbulantColxns.TotalSum;

        internal void ReloadAll()
        {
            IntendedColxns.ReloadFromDB();
            AmbulantColxns.ReloadFromDB();
            Uncollecteds  .ReloadFromDB();
            NoOperations  .ReloadFromDB();
        }
    }
}
