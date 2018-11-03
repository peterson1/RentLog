using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.Version2UI.LeaseBalancesPane.LeasesList;
using System.Collections.Concurrent;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.LeaseBalancesPane
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalancesPaneVM
    {
        private ConcurrentBag<bool> _bag = new ConcurrentBag<bool>();


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
        public string        Status      { get; private set; }
        public IR2Command    ToggleBtn   { get; }


        private async void ToggleRun()
        {
            IsRunning = !IsRunning;
            ToggleBtn.SetLabel(IsRunning ? "Stop" : "Run");
            if (IsRunning) await LeasesList.RefreshAll();
        }


        public void ShowCompleted(LeaseRowVM row)
        {
            _bag.Add(row.Errors.IsBlank());
            var done = _bag.Count;
            var guds = _bag.Count(_ => _);
            var bads = _bag.Count(_ => !_);
            var totl = LeasesList.Count;
            Status = $"Fail: {bads:N0}  :  "
                   + $"Success: {guds:N0}/{done:N0}  :  "
                   + $"Done: {done:N0}/{totl:N0} : {row.Lease}";
        }
    }
}
