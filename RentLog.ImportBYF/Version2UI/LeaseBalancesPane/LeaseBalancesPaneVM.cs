using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList;

namespace RentLog.ImportBYF.Version2UI.LeaseBalancesPane
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalancesPaneVM
    {
        public LeaseBalancesPaneVM(MainWindowVM2 main)
        {
            LeasesList = new LeasesListVM(main);
            ToggleBtn  = R2Command.Relay(ToggleRun, null, "Run");
            ToggleBtn.UpdateLabelOnRun = false;

            main.MasterData.OnAllLeasesMatch += (s, e) =>
            {
                LeasesList.FillLeasesList();
                ToggleRun();
            };
        }


        public LeasesListVM  LeasesList  { get; }
        public bool          IsRunning   { get; private set; }
        public IR2Command    ToggleBtn   { get; }


        private async void ToggleRun()
        {
            IsRunning = !IsRunning;
            ToggleBtn.SetLabel(IsRunning ? "Stop" : "Run");
            if (IsRunning) await LeasesList.RefreshAll();
        }
    }
}
