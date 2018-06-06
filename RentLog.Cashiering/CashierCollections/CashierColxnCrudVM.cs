using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.EnumTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45;
using System;
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
            => AppArgs?.MarketState?.ActiveLeases?.GetAll()?.OrderBy(_ => _.Stall?.Name);


        protected override void ModifyDraftForEditing(CashierColxnDTO draft)
            => draft.Lease = Leases.SingleOrDefault(_ => _.Id == draft.Lease.Id);


        protected override bool IsValidDraft(CashierColxnDTO draft, out string whyInvalid)
        {
            if (draft.DocumentRef.IsBlank())
            {
                whyInvalid = "PR # should not be blank";
                return false;
            }

            if (draft.Amount <= 0)
            {
                whyInvalid = "Amount should be greater than zero";
                return false;
            }

            if (draft.Lease == null)
            {
                whyInvalid = "“Received From” should be an existing lease";
                return false;
            }

            if ((int)draft.BillCode < 1)
            {
                whyInvalid = "“Payment For” should not be blank";
                return false;
            }

            whyInvalid = "";
            return true;
        }


        public    override string TypeDescription => "Tenant Payment to Office";
        protected override string CaptionPrefix   => "Tenant Payment to Office";
    }
}
