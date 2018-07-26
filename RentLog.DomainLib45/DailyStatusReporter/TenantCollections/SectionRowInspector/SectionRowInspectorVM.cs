using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45.SoaViewers.MainWindow;

namespace RentLog.DomainLib45.DailyStatusReporter.TenantCollections
{
    public class SectionRowInspectorVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "";


        public SectionRowInspectorVM(SectionColxnsRow sectionColxnsRow, ITenantDBsDir dir) : base(dir)
        {
            MainRow = sectionColxnsRow;
            EditPRNumberCmd = R2Command.Relay(_ => EditPRNumber(_), null, "Edit PR Number");

            MainRow.Details.ItemOpened += (s, e) => SoaViewer.Show(e.Lease, dir);
            SetCaption($"Collections from “{MainRow?.Section}” by {MainRow.Collector}");
        }


        public SectionColxnsRow  MainRow  { get; }

        public IR2Command  EditPRNumberCmd  { get; }


        private void EditPRNumber(object seleectedItem)
        {
            if (!(seleectedItem is LeaseColxnRow row)) return;
            if (row.DTO == null) throw Null.Ref("LeaseColxnRow.DTO");

            if (!PopUpInput.TryGetInt("PR Number", 
                out int newVal, row.DTO.PRNumber)) return;

            row.DTO.PRNumber = newVal;

            SaveUpdatedRow(row.DTO);
            DailyStatusReportVM.Current.ClickRefresh();
            CloseWindow();
            //todo: trigger main refresh
        }


        private void SaveUpdatedRow(IntendedColxnDTO dto)
        {
            var db   = AppArgs.Collections.For(MainRow.ColxnDate);
            var repo = db.IntendedColxns[MainRow.Section.Id];
            repo.Update(dto);
        }


        public static void Launch(SectionColxnsRow row, ITenantDBsDir dir)
            => new SectionRowInspectorVM(row, dir).Show<SectionRowInspectorWindow>();
    }
}
