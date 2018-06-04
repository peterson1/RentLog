using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.DomainLib45.WithOverduesReport
{
    public class WithOverduesReportVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => WithOverduesReport.TITLE;


        public WithOverduesReportVM(ITenantDBsDir appArguments) : base(appArguments)
        {
            ClickRefresh();
            Rows.ItemOpened += (s, e) => SoaViewer.Show(e.DTO, AppArgs);
        }


        public BillAmounts              Totals  { get; private set; }
        public UIList<LeaseBalanceRow>  Rows    { get; } = new UIList<LeaseBalanceRow>();

        public DateTime AsOfDate      => AppArgs.Collections.LastPostedDate();
        public string   TotalsSummary => $"Total Backrent :  {Totals?.Rent:N2}"
                                        + $"{L.f}Total Overdue Rights :  {Totals?.Rights:N2}";


        protected override async Task OnRefreshClickedAsync()
        {
            List<LeaseBalanceRow> rows = null;
            await Task.Run(() =>
            {
                rows     = AppArgs.Balances.GetOverdueLeases(out BillAmounts totals)
                                           .OrderByDescending(_ => _.Rent).ToList();
                Totals   = totals;
            });
            UIThread.Run(() => Rows.SetItems(rows));
        }
    }


    public static class WithOverduesReport
    {
        public const string TITLE = "With Backrents & Overdue Rights";


        public static void Show(ITenantDBsDir args)
            => new WithOverduesReportVM(args)
                .Show<WithOverduesReportWindow>();


        public static IR2Command CreateLauncherCmd(ITenantDBsDir args)
            => R2Command.Relay(() => Show(args), null, TITLE);
    }
}
