using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList;
using RentLog.ImportBYF.Version2UI.JournalVouchersPane.JVsByDateList;

namespace RentLog.ImportBYF.Version2UI.JournalVouchersPane
{
    [AddINotifyPropertyChangedInterface]
    public class JournalVouchersPaneVM
    {
        public JournalVouchersPaneVM(MainWindowVM2 mainWindowVM2)
        {
            Rows      = new JVsByDateListVM(mainWindowVM2);
            ToggleCmd = R2Command.Relay(ToggleRun, null, "Run");
            ToggleCmd.UpdateLabelOnRun = false;
        }


        public JVsByDateListVM  Rows        { get; }
        public bool             IsRunning   { get; private set; }
        public IR2Command       ToggleCmd   { get; }
        public string           Status      { get; set; }
        public bool             IsReversed  { get; set; }


        private async void ToggleRun()
        {
            Status    = "";
            IsRunning = !IsRunning;
            ToggleCmd.SetLabel(IsRunning ? "Stop" : "Run");
            if (!IsRunning) return;

            if (IsReversed)
                await Rows.RefreshAllReversed();
            else
                await Rows.RefreshAll();
        }
    }
}
