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

namespace RentLog.Cashiering.OtherCollections
{
    public class OtherColxnCrudVM : RepoCrudWindowVMBase<IOtherColxnsRepo, OtherColxnDTO, OtherColxnCrudWindow, ITenantDBsDir>
    {
        public OtherColxnCrudVM(IOtherColxnsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
            Collectors.SetItems(AppArgs.MarketState.Collectors?.GetAll());
            GLAccounts.SetItems(AppArgs.MarketState.GLAccounts?.GetAll());
        }

                                                 
        public UIList<CollectorDTO>  Collectors  { get; } = new UIList<CollectorDTO>();
        public UIList<GLAccountDTO>  GLAccounts  { get; } = new UIList<GLAccountDTO>();


        protected override void ModifyDraftForUpdating(OtherColxnDTO draft)
        {
            draft.Collector = Collectors.SingleOrDefault(_ => _.Id == draft.Collector.Id);
            draft.GLAccount = GLAccounts.SingleOrDefault(_ => _.Id == draft.GLAccount.Id);
        }


        public    override string TypeDescription => "Other Collection";
        protected override string CaptionPrefix   => "Other Collection";
    }
}
