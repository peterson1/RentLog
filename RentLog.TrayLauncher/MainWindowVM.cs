using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.LicenseTools;
using System;
using System.IO;
using System.Windows.Input;
using static RentLog.TrayLauncher.Properties.Settings;


namespace RentLog.TrayLauncher
{
    public class MainWindowVM : UpdatedExeVMBase<LauncherArguments>
    {
        protected override string CaptionPrefix => Default.TrayToolTip;
        public const string MKT_DB = "MarketState.ldb";


        public MainWindowVM(LauncherArguments appArguments) : base(appArguments)
        {
            if (TryParseCredentials())
            {
                EncoderCmd      = NewCmd("RL.Cashier.exe", "Encode Collections");
                ReviewerCmd     = NewCmd("RL.Cashier.exe", "Review Collections");
                StallsCmd       = NewCmd("RL.Stalls.exe" , "Stalls & Sections");
                LeasesCmd       = NewCmd("RL.Leases.exe" , "Tenants & Leases");
                ChequesCmd      = NewCmd("RL.Cheques.exe", "Vouchers & DCDR");
                OverduesCmd     = NewCmd("RL.Reports.exe", "Backrentals & Overdue Rights", "Overdues");
                ColxnSummaryCmd = NewCmd("RL.Reports.exe", "Collections Summary Report"  , "ColxnSmry");
                DailyStatusCmd  = NewCmd("RL.Reports.exe", "Daily Status Report"         , "DailyStatus");
            }
            UpdateNotifier.ExecuteOnFileChanged = true;
        }


        public string ToolTipText { get; set; } = Default.TrayToolTip;


        public ExeLauncherCommand   EncoderCmd       { get; }
        public ExeLauncherCommand   ReviewerCmd      { get; }
                                                     
        public ExeLauncherCommand   StallsCmd        { get; }
        public ExeLauncherCommand   LeasesCmd        { get; }
        public ExeLauncherCommand   ChequesCmd       { get; }
                                                     
        public ExeLauncherCommand   OverduesCmd      { get; }
        public ExeLauncherCommand   ColxnSummaryCmd  { get; }
        public ExeLauncherCommand   DailyStatusCmd   { get; }
                                                     
        public string               NameAndRole      { get; private set; } = "verifying ...";
        public string               ArgumentError    { get; private set; }


        private bool TryParseCredentials()
        {
            var ok = SeatLicenser.TryGetCredentials(AppArgs.CredentialsKey, 
                        out FirebaseCredentials creds, out string err);
            if (ok) NameAndRole = creds.NameAndRole;
            ArgumentError = err;
            return ok;
        }


        private ExeLauncherCommand NewCmd(string exeName, string btnLabel, string param1 = null)
        {
            var cmdArgs = ComposeArgs(exeName, out string exePath, param1);
            return new TempExeLauncherCommand(btnLabel, exePath, cmdArgs);
        }


        private string ComposeArgs(string exeName, out string exePath, string param1)
        {
            exePath = Path.Combine(AppArgs.UpdatedExeDir, exeName);
            var mktDb = Path.Combine(AppArgs.DatabasesPath, MKT_DB);
            var arg = $"-db=\"{mktDb}\""
                    + $" -key=\"{AppArgs.CredentialsKey}\"" 
                    + $" -exe=\"{exePath}\"";

            if (!param1.IsBlank())
                arg += $" -p1=\"{param1}\"";

            return arg;
        }
    }
}
