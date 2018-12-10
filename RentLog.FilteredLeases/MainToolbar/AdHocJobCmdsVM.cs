using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.AdHocJobs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using System;
using System.Threading.Tasks;

namespace RentLog.FilteredLeases.MainToolbar
{
    public class AdHocJobCmdsVM
    {
        private ITenantDBsDir _dir;
        private MainWindowVM  _main;

        public AdHocJobCmdsVM(MainWindowVM mainWindowVM)
        {
            _main        = mainWindowVM;
            _dir         = _main.AppArgs;
            AdHocJobCmd1 = R2Command.Relay(_ => RunAdHoc(1), _ => _dir.CanRunAdHocTask(false), "Run Ad Hoc Command 1");
            AdHocJobCmd2 = R2Command.Relay(_ => RunAdHoc(2), _ => _dir.CanRunAdHocTask(false), "Run Ad Hoc Command 2");
            AdHocJobCmd3 = R2Command.Relay(_ => RunAdHoc(3), _ => _dir.CanRunAdHocTask(false), "Run Ad Hoc Command 3");
        }


        public IR2Command  AdHocJobCmd1  { get; }
        public IR2Command  AdHocJobCmd2  { get; }
        public IR2Command  AdHocJobCmd3  { get; }


        private void RunAdHoc(int taskNumber)
        {
            Action adhocJob; string desc;
            switch (taskNumber)
            {
                case 1: adhocJob =
                    ForActiveLeases.RebuildSoA(_dir, out desc);
                    break;

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
    }
}
