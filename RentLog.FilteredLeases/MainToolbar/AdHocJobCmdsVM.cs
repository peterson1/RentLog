using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.AdHocJobs;
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
            AdHocJobCmd1 = R2Command.Relay(_ => RunAdHoc(1), null, "Run Ad Hoc Command 1");
            AdHocJobCmd2 = R2Command.Relay(_ => RunAdHoc(2), null, "Run Ad Hoc Command 2");
            AdHocJobCmd3 = R2Command.Relay(_ => RunAdHoc(3), null, "Run Ad Hoc Command 3");
            AdHocJobCmd4 = R2Command.Relay(_ => RunAdHoc(4), null, "Run Ad Hoc Command 4");
        }


        public IR2Command  AdHocJobCmd1  { get; }
        public IR2Command  AdHocJobCmd2  { get; }
        public IR2Command  AdHocJobCmd3  { get; }
        public IR2Command  AdHocJobCmd4  { get; }


        private void RunAdHoc(int taskNumber)
        {
            Action adhocJob; string desc; bool canRun;
            switch (taskNumber)
            {
                case 1: adhocJob =
                        ForActiveLeases.RebuildSoA(_dir, out desc, out canRun);
                        break;
                case 2: adhocJob =
                        SectionNight.SetNoSurcharge(_dir, out desc, out canRun);
                        break;
                case 3: adhocJob =
                        StallsJob.SetStallDefaults(_dir, out desc, out canRun);
                        break;
                case 4: adhocJob =
                        ForActiveLeases.Reprocess3DaysBack(_dir, out desc, out canRun);
                        break;
                default: throw Bad.Data($"Task #: [{taskNumber}]");
            }

            if (!canRun)
            {
                var creds = _dir.Credentials;
                Alert.ShowModal("Not Authorized to Execute",
                    $"“{creds.HumanName}” ({creds.Roles}) {L.f} is not allowed to {L.f} “{desc}”.");
                return;
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
