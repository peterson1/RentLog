using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.EnumTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.Cashiering.CashierCollections
{
    public class CashierColxnCrudVM : RepoCrudWindowVMBase<ICashierColxnsRepo, CashierColxnDTO, CashierColxnCrudWindow, ITenantDBsDir>
    {
        public CashierColxnCrudVM(ICashierColxnsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
            Leases   .SetItems(GetSortedLeases());
            BillCodes.SetItems(EnumTool.List<BillCode>());
        }


        public UIList<LeaseDTO>  Leases     { get; } = new UIList<LeaseDTO>();
        public UIList<BillCode>  BillCodes  { get; } = new UIList<BillCode>();


        private IEnumerable<LeaseDTO> GetSortedLeases()
        {
            var mkt = AppArgs?.MarketState;
            if (mkt == null) return null;
            var date = AppArgs.Collections.UnclosedDate();
            return mkt.ActiveLeasesFor(date)
                      .OrderBy(_ => _.Stall?.Name);
        }

        protected override void ModifyDraftForUpdating(CashierColxnDTO draft)
            => draft.Lease = Leases.SingleOrDefault(_ => _.Id == draft.Lease.Id);


        public    override string TypeDescription => "Tenant Payment to Office";
        protected override string CaptionPrefix   => "Tenant Payment to Office";
    }
}
