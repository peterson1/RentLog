using CommonTools.Lib11.FileSystemTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.Configuration;
using System.IO;
using static RentLog.TrayLauncher.Properties.Settings;


namespace RentLog.TrayLauncher
{
    public class LauncherArguments : IHasUpdatedCopy, ILauncherParams
    {
        public string DatabasesPath   => Default.DatabasesPath;
        public string CredentialsKey  => Default.CredentialsKey;
        public string UpdatedCopyPath => Default.UpdatedExeSource;
        public string UpdatedExeDir   => UpdatedCopyPath.IsBlank() ? "" 
                                       : Path.GetDirectoryName(UpdatedCopyPath);
    }
}
