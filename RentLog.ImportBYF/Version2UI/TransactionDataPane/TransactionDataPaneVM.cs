using System;
using System.Linq;
using System.Threading.Tasks;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.DomainLib11.StateTransitions;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;

namespace RentLog.ImportBYF.Version2UI.TransactionDataPane
{
    [AddINotifyPropertyChangedInterface]
    public class TransactionDataPaneVM
    {
        public TransactionDataPaneVM(MainWindowVM2 main)
        {
            PeriodsList  = new PeriodsListVM(main);
            RemediateCmd = R2Command.Async(Remediate, null, "Remediate");
            ToggleBtn    = R2Command.Relay(ToggleRun, null, "Run");
            ToggleBtn.UpdateLabelOnRun = false;

            main.ByfServer.GotMinMaxDates += (s, e) =>
            {
                PeriodsList.FillPeriodsList(e);
                //ToggleRun();
            };
        }


        public PeriodsListVM  PeriodsList   { get; }
        public bool           IsRunning     { get; private set; }
        public IR2Command     ToggleBtn     { get; }
        public IR2Command     RemediateCmd  { get; }


        private async void ToggleRun()
        {
            IsRunning = !IsRunning;
            ToggleBtn.SetLabel(IsRunning ? "Stop" : "Run");
            if (IsRunning) await PeriodsList.RefreshAll();
        }


        private async Task Remediate()
        {
            var args = PeriodsList.First().MainWindow.AppArgs;
            foreach (var row in PeriodsList)
            {
                row.StartBeingBusy("Adding deposits to passbook ...");
                await Task.Run(() 
                    => args.AddDepositsToPassbook(row.Date));
                row.StopBeingBusy();
            }
        }
    }
}
