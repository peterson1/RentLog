using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;

namespace RentLog.DomainLib45.CollectorsCRUD.MainEditor
{
    public class MainEditorVM : RepoCrudWindowVMBase<ICollectorsRepo, CollectorDTO, MainEditorWindow, ITenantDBsDir>
    {
        public MainEditorVM(ICollectorsRepo repository, ITenantDBsDir appArguments) : base(repository, appArguments)
        {
        }


        protected override CollectorDTO GetNewDraft() => new CollectorDTO
        {
            IsActive = true,
        };


        protected override string CaptionPrefix       => "Collector Editor";
        public    override string TypeDescription     => $"{AppArgs.MarketState.SystemName} Collector";
        public    override bool   CanEncodeNewDraft() => AppArgs.CanAddCollector(false);
    }
}
