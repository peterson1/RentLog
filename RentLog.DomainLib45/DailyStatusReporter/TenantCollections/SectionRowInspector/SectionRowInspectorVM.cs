using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.Authorization;
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
            EditPRNumberCmd = R2Command.Relay(_ => EditPRNumber(_), _ => CanEditPRNumber(), "Edit PR Number");

            MainRow.Details.ItemOpened += (s, e) => SoaViewer.Show(e.Lease, dir);
            SetCaption($"Collections from “{MainRow?.Section}” by {MainRow.Collector}");
        }


        public SectionColxnsRow  MainRow  { get; }

        public IR2Command  EditPRNumberCmd  { get; }


        private bool CanEditPRNumber()
            => AppArgs.CanEditPostedPRNumber(false);


        private void EditPRNumber(object seleectedItem)
        {
            if (!(seleectedItem is LeaseColxnRow row)) return;
            if (row.IntendedDTO == null && row.AmbulantDTO == null)
                throw Null.Ref("LeaseColxnRow.DTO");

            var oldVal = row.IsAmbulant ? row.AmbulantDTO.PRNumber
                                        : row.IntendedDTO.PRNumber;

            if (!PopUpInput.TryGetInt("PR Number", 
                out int newVal, oldVal)) return;

            row.DocumentRef = newVal.ToString();

            SaveUpdatedRow(row);
            DailyStatusReportVM.Current.ClickRefresh();
            CloseWindow();
        }


        private void SaveUpdatedRow(LeaseColxnRow row)
        {
            var db    = AppArgs.Collections.For(MainRow.ColxnDate);
            //var repo = db.IntendedColxns[MainRow.Section.Id];
            //repo.Update(dto);
            var secID = MainRow.Section.Id;
            if (row.IsAmbulant)
            {
                row.AmbulantDTO.PRNumber = int.Parse(row.DocumentRef);
                db.AmbulantColxns[secID].Update(row.AmbulantDTO);
            }
            else
            {
                row.IntendedDTO.PRNumber = int.Parse(row.DocumentRef);
                db.IntendedColxns[secID].Update(row.IntendedDTO);
            }

        }


        public static void Launch(SectionColxnsRow row, ITenantDBsDir dir)
            => new SectionRowInspectorVM(row, dir).Show<SectionRowInspectorWindow>();
    }
}
