using CommonTools.Lib45.BaseViewModels;
using static RentLog.TrayLauncher.Properties.Settings;


namespace RentLog.TrayLauncher
{
    public class MainWindowVM : UpdatedExeVMBase<LauncherArguments>
    {
        protected override string CaptionPrefix => Default.TrayToolTip;


        public MainWindowVM(LauncherArguments appArguments) : base(appArguments)
        {
            UpdateNotifier.ExecuteOnFileChanged = true;
        }
    }
}
