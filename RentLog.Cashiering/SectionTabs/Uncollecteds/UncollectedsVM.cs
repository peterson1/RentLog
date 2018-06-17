using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.BillingRules;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    [AddINotifyPropertyChangedInterface]
    public class UncollectedsVM : EncoderListVMBase<UncollectedLeaseDTO>
    {
        private SectionTabVM _tab;


        public UncollectedsVM(SectionTabVM sectionTabVM, MainWindowVM main) 
            : base(GetRepo(sectionTabVM, main), main)
        {
            _tab         = sectionTabVM;
            CanAddRows   = false;
            TotalVisible = false;
        }


        private List<UncollectedLeaseDTO> GetUpdatedUncollecteds(ISimpleRepo<UncollectedLeaseDTO> db)
        {
            var repo = db as IUncollectedsRepo;
            var list = repo.InferUncollecteds(_tab.IntendedColxns.ItemsList,
                                              _tab.NoOperations.ItemsList);
            db.Drop();
            db.Insert(list, false);
            return list;
        }


        //protected override List<UncollectedLeaseDTO> QueryItems(ISimpleRepo<UncollectedLeaseDTO> db)
        //    => Main.CanEncode ? GetUpdatedUncollecteds(db)
        //                      : base.QueryItems(db);


        private static ISimpleRepo<UncollectedLeaseDTO> GetRepo(SectionTabVM tab, MainWindowVM main)
            => main.ColxnsDB.Uncollecteds[tab.Section.Id];


        protected override string ListTitle => "Uncollecteds";
        protected override Func<UncollectedLeaseDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        protected override Func<UncollectedLeaseDTO, decimal>  SummedAmount => _ => _.Targets.Total;

    }
}
