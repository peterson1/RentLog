using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.Cashiering.CommonControls
{
    [AddINotifyPropertyChangedInterface]
    public abstract class EncoderListVMBase<TDTO> 
        : SavedListVMBase<TDTO, ITenantDBsDir>
        where TDTO : IDocumentDTO
    {
        public event EventHandler<CollectorDTO> CurrentCollectorChanged = delegate { };


        public EncoderListVMBase(ISimpleRepo<TDTO> repository, MainWindowVM mainWindowVM) : base(repository, mainWindowVM.AppArgs, false)
        {
            Main       = mainWindowVM;
            CanAddRows = Main.CanEncode;
            Caption    = ListTitle;
            
            //if (refreshMainOnUpdatedTotal)
            TotalSumChanged += (s, e) => OnTotalSumChanged();
        }

                                                      
        protected abstract string    ListTitle        { get; }
        public MainWindowVM          Main             { get; }
        public bool                  CanAddRows       { get; protected set; }
        public bool                  TotalVisible     { get; protected set; } = true;
        public bool                  ShowCollectors   { get; private set; }
        public UIList<CollectorDTO>  Collectors       { get; } = new UIList<CollectorDTO>();
        public CollectorDTO          CurrentCollector { get; set; }


        protected virtual void OnTotalSumChanged() => Main.ClickRefresh();


        public void OnCurrentCollectorChanged()
            => CurrentCollectorChanged?.Invoke(this, CurrentCollector);
            //Main.ColxnsDB.SetCollector(_tab.Section, CurrentCollector);


        protected override IEnumerable<TDTO> PostProcessQueried(IEnumerable<TDTO> items)
        {
            if (LeaseGetter != null)
            {
                foreach (var item in items)
                    AppArgs.MarketState.RefreshStall(LeaseGetter(item));
            }
            return items;
        }


        protected void FillCollectorsList(CollectorDTO defaultCollector)
        {
            Collectors.SetItems(AppArgs.MarketState.Collectors.GetAll());

            if (defaultCollector != null)
                CurrentCollector = Collectors.SingleOrDefault(_ => _.Id == defaultCollector.Id);

            ShowCollectors   = true;
        }


        protected virtual Func<TDTO, LeaseDTO> LeaseGetter => null;
        public    override bool CanEditRecord    (TDTO rec) => Main.CanEncode;
        protected override bool CanDeleteRecord (TDTO rec) => Main.CanEncode;
    }
}
