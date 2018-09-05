using System;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45.SoaViewers.MainWindow;

namespace RentLog.DomainLib45.DailyStatusReporter.CollectorsPerformance.MainRowInspector
{
    [AddINotifyPropertyChangedInterface]
    public class MainRowInspectorVM : MainWindowVMBase<CollectorPerformanceRow>
    {
        protected override string CaptionPrefix => $"{AppArgs.Collector} for {AppArgs.Assignment}";
        private ITenantDBsDir _dir;


        public MainRowInspectorVM(CollectorPerformanceRow appArguments, ITenantDBsDir tenantDBsDir) : base(appArguments)
        {
            _dir = tenantDBsDir;
            AppArgs.ItemOpened += (s, e) => OnItemOpened(e);
        }


        private void OnItemOpened(CollectorPerfSubRow e)
        {
            _dir.MarketState.RefreshStall(e.Lease);
            SoaViewer.Show(e.Lease, _dir);
        }

        public static void Launch(CollectorPerformanceRow row, ITenantDBsDir dir)
            => new MainRowInspectorVM(row, dir).Show<MainRowInspectorWindow>();
    }
}
