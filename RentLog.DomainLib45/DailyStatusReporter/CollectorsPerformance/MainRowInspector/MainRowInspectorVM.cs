using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;

namespace RentLog.DomainLib45.DailyStatusReporter.CollectorsPerformance.MainRowInspector
{
    public class MainRowInspectorVM : MainWindowVMBase<CollectorPerformanceRow>
    {
        protected override string CaptionPrefix => $"{AppArgs.Collector} for {AppArgs.Assignment}";


        public MainRowInspectorVM(CollectorPerformanceRow appArguments) : base(appArguments)
        {
        }


        public static void Launch(CollectorPerformanceRow row)
            => new MainRowInspectorVM(row).Show<MainRowInspectorWindow>();
    }
}
