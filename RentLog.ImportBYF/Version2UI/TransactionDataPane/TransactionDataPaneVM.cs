using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;

namespace RentLog.ImportBYF.Version2UI.TransactionDataPane
{
    [AddINotifyPropertyChangedInterface]
    public class TransactionDataPaneVM
    {
        public TransactionDataPaneVM(MainWindowVM2 main)
        {
            PeriodsList = new PeriodsListVM(main);
            ToggleBtn   = R2Command.Relay(ToggleRun, null, "Run");
            ToggleBtn.UpdateLabelOnRun = false;

            main.ByfServer.GotMinMaxDates += (s, e) =>
            {
                PeriodsList.FillPeriodsList(e);
                ToggleRun();
            };
        }


        public PeriodsListVM  PeriodsList  { get; }
        public bool           IsRunning    { get; private set; }
        public IR2Command     ToggleBtn    { get; }


        private async void ToggleRun()
        {
            IsRunning = !IsRunning;
            ToggleBtn.SetLabel(IsRunning ? "Stop" : "Run");
            if (IsRunning) await PeriodsList.RefreshAll();
        }
    }
}
