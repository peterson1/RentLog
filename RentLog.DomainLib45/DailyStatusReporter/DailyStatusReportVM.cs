using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.PrintTools;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib45.DailyStatusReporter.PrintLayouts;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RentLog.DomainLib45.DailyStatusReporter
{
    public class DailyStatusReportVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "Daily Status";
        public static DailyStatusReportVM Current;


        public DailyStatusReportVM(ITenantDBsDir dir) : base(dir)
        {
            MinDate         = dir.Collections.AllDates().First();
            MaxDate         = dir.Collections.LastPostedDate();
            ReportDate      = MaxDate;
            PreviousDateCmd = R2Command.Relay(() => ReportDate = ReportDate.AddDays(-1),
                                               _ => ReportDate > MinDate, "Previous Day");
            NextDateCmd     = R2Command.Relay(() => ReportDate = ReportDate.AddDays(1),
                                               _ => ReportDate < MaxDate, "Next Day");
        }



        public DateTime           MinDate     { get; }
        public DateTime           MaxDate     { get; }
        public DateTime           ReportDate  { get; set; }
        public DailyStatusReport  MainReport  { get; private set; }

        public IR2Command   PreviousDateCmd  { get; }
        public IR2Command   NextDateCmd      { get; }


        public void OnReportDateChanged() => ClickRefresh();


        protected override void OnRefreshClicked()
        {
            MainReport = null;
            try
            {
                MainReport = new DailyStatusReport(ReportDate, AppArgs);
            }
            catch (Exception ex)
            {
                Alert.Show(ex);
            }
        }


        protected override async void OnPrintClicked()
        {
            var win                = new PrintWindow1();
            win.DataContext        = this.MainReport;
            win.printPanel.Padding = new Thickness(30, 60, 30, 30);
            win.Show();
            await Task.Delay(1000 * 1);
            PrintPreviewer.FitTo(8.5, 11, win.printPanel);
            win.Close();
        }


        public static void Launch(ITenantDBsDir dir)
        {
            Current = new DailyStatusReportVM(dir);
            Current.Show<DailyStatusReportWindow>();
        }
    }
}
