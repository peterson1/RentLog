using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Reporters;
using System;
using System.Linq;

namespace RentLog.DomainLib45.DailyStatusReporter
{
    public class DailyStatusReportVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "Daily Status";


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
            MainReport = new DailyStatusReport(ReportDate, AppArgs);
        }


        public static void Launch(ITenantDBsDir dir)
            => new DailyStatusReportVM(dir).Show<DailyStatusReportWindow>();
    }
}
