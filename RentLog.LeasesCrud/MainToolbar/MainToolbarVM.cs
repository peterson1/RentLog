using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib11.ExceptionTools;
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
            //RunAdHocTask1Cmd      = R2Command.Relay(_ => RunAdHocTask(1), _ => _args.CanRunAdHocTask(false), "Run Ad Hoc Script 1");
            //RunAdHocTask2Cmd      = R2Command.Relay(_ => RunAdHocTask(2), _ => _args.CanRunAdHocTask(false), "Run Ad Hoc Script 2");
            //RunAdHocTask3Cmd      = R2Command.Relay(_ => RunAdHocTask(3), _ => _args.CanRunAdHocTask(false), "Run Ad Hoc Script 3");
        }


        public IR2Command   WithOverduesReportCmd  { get; }
        public IR2Command   PrintCurrentListCmd    { get; }
        public IR2Command   ExportListToExcelCmd   { get; }
        //public IR2Command   RunAdHocTask1Cmd       { get; }
        //public IR2Command   RunAdHocTask2Cmd       { get; }
        //public IR2Command   RunAdHocTask3Cmd       { get; }
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
        {
            try
            {
                Overdues = _args.Balances.TotalOverdues();
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "Getting total overdues");
            }
        }

        /*private void RunAdHocTask(int taskNumber)
        {
            Action adhocJob; string desc;

            switch (taskNumber)
            {
                case 1: adhocJob = GetAdHocJob1(out desc); break;
                case 2: adhocJob = GetAdHocJob2(out desc); break;
                case 3: adhocJob = GetAdHocJob3(out desc); break;
                default: throw Bad.Data($"Task #: [{taskNumber}]");
            }

            Alert.Confirm($"Run Ad Hoc job “{desc}”?", async () =>
            {
                _main.StartBeingBusy("Running Ad Hoc task ...");

                await Task.Run(() => adhocJob.Invoke());

                _main.StopBeingBusy();
                _main.ClickRefresh();
            });
        }


        private Action GetAdHocJob1(out string desc)
        {
            desc = "MarketRenovation.DecommissionOldStalls";

            // solo task job
            return () => MarketRenovation.DecommissionOldStalls(_args, 3, 29.Nov(2018));

            // multi-job
            //var jobs = MarketRenovation.TerminateOldLeases(_args, 3, 29.Nov(2018));

            // multi-job parallel
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
            //        job.Invoke();
            //};
        }


        private Action GetAdHocJob2(out string desc)
        {
            desc = "ForActiveLeases.RebuildSoA";
            return () => ForActiveLeases.RebuildSoA(_args);
        }


        private Action GetAdHocJob3(out string desc)
        {
            desc = "ForInactiveLeases.RebuildSoA";
            return () => ForInactiveLeases.RebuildSoA(_args);
        }*/
    }
}
