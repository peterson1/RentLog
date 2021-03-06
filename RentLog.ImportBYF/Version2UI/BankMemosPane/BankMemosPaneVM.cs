﻿using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.ImportBYF.Version2UI.BankMemosPane.MemosByDateList;

namespace RentLog.ImportBYF.Version2UI.BankMemosPane
{
    [AddINotifyPropertyChangedInterface]
    public class BankMemosPaneVM
    {
        public BankMemosPaneVM(MainWindowVM2 mainWindowVM2)
        {
            Rows      = new MemosByDateListVM(mainWindowVM2);
            ToggleCmd = R2Command.Relay(ToggleRun, null, "Run");
            ToggleCmd.UpdateLabelOnRun = false;
        }


        public MemosByDateListVM   Rows        { get; }
        public bool                IsRunning   { get; private set; }
        public IR2Command          ToggleCmd   { get; }
        public string              Status      { get; set; }
        public bool                IsReversed  { get; set; }


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
