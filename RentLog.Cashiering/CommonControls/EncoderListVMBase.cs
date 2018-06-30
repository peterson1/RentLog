using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.Cashiering.CommonControls
{
    public abstract class EncoderListVMBase<TDTO> 
        : SavedListVMBase<TDTO, ITenantDBsDir>
        where TDTO : IDocumentDTO
    {
        public EncoderListVMBase(ISimpleRepo<TDTO> repository, MainWindowVM mainWindowVM) : base(repository, mainWindowVM.AppArgs, false)
        {
            Main       = mainWindowVM;
            CanAddRows = Main.CanEncode;
            Caption    = ListTitle;

            //if (refreshMainOnUpdatedTotal)
            TotalSumChanged += (s, e) => OnTotalSumChanged();
        }


        public MainWindowVM  Main          { get; }
        public bool          CanAddRows    { get; protected set; }
        public bool          TotalVisible  { get; protected set; } = true;

        protected abstract string ListTitle { get; }

        protected virtual void OnTotalSumChanged() => Main.ClickRefresh();


        protected override IEnumerable<TDTO> PostProcessQueried(IEnumerable<TDTO> items)
        {
            if (LeaseGetter != null)
            {
                foreach (var item in items)
                    AppArgs.MarketState.RefreshStall(LeaseGetter(item));
            }
            return items;
        }


        protected virtual Func<TDTO, LeaseDTO> LeaseGetter => null;
        public    override bool CanEditRecord    (TDTO rec) => Main.CanEncode;
        protected override bool CanDeleteRecord (TDTO rec) => Main.CanEncode;
    }
}
