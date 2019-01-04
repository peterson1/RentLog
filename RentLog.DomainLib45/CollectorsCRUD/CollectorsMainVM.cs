using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib45.CollectorsCRUD.MainList;
using RentLog.DomainLib45.CollectorsCRUD.MainToolbar;

namespace RentLog.DomainLib45.CollectorsCRUD
{
    [AddINotifyPropertyChangedInterface]
    public class CollectorsMainVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => $"{AppArgs.MarketState.SystemName} Collectors";


        public CollectorsMainVM(ITenantDBsDir appArguments) : base(appArguments)
        {
            var mkt           = appArguments.MarketState;
            Rows              = new MainListVM(mkt.Collectors, appArguments);
            ToolBar           = new MainToolbarVM(this);
            EncodeNewDraftCmd = R2Command.Relay(EncodeNewDraft);
        }


        public MainToolbarVM  ToolBar            { get; }
        public MainListVM     Rows               { get; }
        public IR2Command     EncodeNewDraftCmd  { get; }


        private void EncodeNewDraft()
        {
            Alert.ShowModal("???", "---");
        }


        protected override void OnWindowLoaded() 
            => Rows.ClickRefresh();


        public static void Launch(ITenantDBsDir tenantDBsDir)
            => new CollectorsMainVM(tenantDBsDir).Show<CollectorsMainWindow>();
    }
}
