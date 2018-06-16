using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    [AddINotifyPropertyChangedInterface]
    public class UncollectedsVM : EncoderListVMBase<UncollectedLeaseDTO>
    {
        protected override string ListTitle => "Uncollecteds";


        public UncollectedsVM(SectionTabVM sectionTabVM, MainWindowVM main) 
            : base(GetRepo(sectionTabVM, main), main)
        {
            CanAddRows   = false;
            TotalVisible = false;
        }


        private static ISimpleRepo<UncollectedLeaseDTO> GetRepo(SectionTabVM tab, MainWindowVM main)
            => main.ColxnsDB.Uncollecteds[tab.Section.Id];


        protected override List<UncollectedLeaseDTO> QueryItems(ISimpleRepo<UncollectedLeaseDTO> db)
            => Main.CanEncode ? GetUpdatedUncollecteds(db)
                              : base.QueryItems(db);


        private List<UncollectedLeaseDTO> GetUpdatedUncollecteds(ISimpleRepo<UncollectedLeaseDTO> db)
        {
            throw new NotImplementedException();
            //todo: replace contents of Uncollecteds
        }


        protected override Func<UncollectedLeaseDTO, LeaseDTO> LeaseGetter  => _ => _.Lease;
        protected override Func<UncollectedLeaseDTO, decimal>  SummedAmount => _ => _.Targets.Total;

    }
}
