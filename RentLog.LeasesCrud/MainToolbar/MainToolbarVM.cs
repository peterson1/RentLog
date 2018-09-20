using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.ExcelTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.PrintTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.AdHocJobs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45.WithOverduesReport;
using System;
using System.Threading.Tasks;

namespace RentLog.LeasesCrud.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private MainWindowVM  _main;
        private ITenantDBsDir _args;


        public MainToolbarVM(MainWindowVM mainWindowVM)
        {
            _main                 = mainWindowVM;
            _args                 = _main.AppArgs;
            WithOverduesReportCmd = WithOverduesReport.CreateLauncherCmd(_args);
            PrintCurrentListCmd   = R2Command.Relay(PrintCurrentList, null, "Print Current List");
            ExportListToExcelCmd  = R2Command.Relay(ExportListToExcel, null, "Export List to Excel");
            RunAdHocTaskCmd       = R2Command.Relay(RunAdHocTask, _ => _args.CanRunAdHocTask(false), "Run Ad Hoc Script");
        }


        public IR2Command   WithOverduesReportCmd  { get; }
        public IR2Command   PrintCurrentListCmd    { get; }
        public IR2Command   ExportListToExcelCmd   { get; }
        public IR2Command   RunAdHocTaskCmd        { get; }
        public BillAmounts  Overdues               { get; private set; }


        private void PrintCurrentList()
        {
            var labl = _main.CurrentIsActive ? "Acive Leases"
                                             : "Inacive Leases";
            _main.CurrentTable.AskToPrint
                (labl, null, _args.MarketState.BranchName);
        }


        private void ExportListToExcel() 
            => _main.CurrentTable.ExportToExcel();


        public void UpdateAll() 
            => Overdues = _args.Balances.TotalOverdues();


        private void RunAdHocTask()
        {
            var adhocJob = GetAdHocJob(out string desc);
            Alert.Confirm($"Run Ad Hoc job “{desc}”?", async () =>
            {
                _main.StartBeingBusy("Running Ad Hoc task ...");

                await Task.Run(() => adhocJob.Invoke());

                _main.StopBeingBusy();
                _main.ClickRefresh();
            });
        }


        private Action GetAdHocJob(out string desc)
        {
            desc = "ForActiveLeases.ResetStart(21.Sep(2018)";

            // multi-job parallel
            //var jobs = ForAllLeases.RecomputeAllBalances(_args);
            //return jobs.AsParallelJob((ok, not, total) =>
            //{
            //    var left = total - (ok + not);
            //    _main.StartBeingBusy($"success: {ok}"
            //                 + L.f + $"failed: {not}"
            //                 + L.f + $"total: {total}"
            //                 + L.f + $"left: {left}");
            //});
            
            // multi-job serial
            //return () =>
            //{
            //    foreach (var job in jobs)
            //    {
            //        job.Invoke();
            //    }
            //};

            // solo task job
            return () => ForActiveLeases.ResetStart(21.Sep(2018), _args);
        }
    }
}
