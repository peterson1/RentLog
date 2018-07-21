using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.ReportRows;

namespace RentLog.DomainLib45.DailyStatusReporter.TenantCollections
{
    public class SectionRowInspectorVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "";


        public SectionRowInspectorVM(SectionColxnsRow sectionColxnsRow, ITenantDBsDir appArguments) : base(appArguments)
        {
            MainRow = sectionColxnsRow;
            SetCaption($"Collections from “{MainRow?.Section}” by {MainRow.Collector}");
        }


        public SectionColxnsRow  MainRow  { get; }


        public static void Launch(SectionColxnsRow row, ITenantDBsDir dir)
            => new SectionRowInspectorVM(row, dir).Show<SectionRowInspectorWindow>();
    }
}
