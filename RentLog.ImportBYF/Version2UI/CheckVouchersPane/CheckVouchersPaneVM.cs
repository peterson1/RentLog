using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.Version2UI.CheckVouchersPane.CVsByDateList;

namespace RentLog.ImportBYF.Version2UI.CheckVouchersPane
{
    [AddINotifyPropertyChangedInterface]
    public class CheckVouchersPaneVM
    {
        public CheckVouchersPaneVM(MainWindowVM2 mainWindowVM2)
        {
            Rows      = new CVsByDateListVM(mainWindowVM2);
            ToggleCmd = R2Command.Relay(ToggleRun, null, "Run");
            ToggleCmd.UpdateLabelOnRun = false;
        }


        public CVsByDateListVM  Rows       { get; }
        public bool             IsRunning  { get; private set; }
        public IR2Command       ToggleCmd  { get; }
        public string           Status     { get; set; }


        private async void ToggleRun()
        {
            Status    = "";
            IsRunning = !IsRunning;
            ToggleCmd.SetLabel(IsRunning ? "Stop" : "Run");
            if (IsRunning) await Rows.RefreshAll();
        }
    }
}
