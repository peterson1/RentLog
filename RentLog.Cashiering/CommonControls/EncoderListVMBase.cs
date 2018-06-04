using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;
using System.Collections.Generic;

namespace RentLog.Cashiering.CommonControls
{
    public abstract class EncoderListVMBase<TDTO> : SavedListVMBase<TDTO, ITenantDBsDir>
        where TDTO : IDocumentDTO
    {
        public EncoderListVMBase(ISimpleRepo<TDTO> repository, ITenantDBsDir tenantDBsDir) : base(repository, tenantDBsDir, false)
        {
            CanAddRows = AppArgs.CanEncodeCollections(false);
            Caption    = ListTitle;
        }


        public bool  CanAddRows    { get; protected set; }
        public bool  TotalVisible  { get; protected set; } = true;


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


        protected abstract string ListTitle { get; }
    }
}
