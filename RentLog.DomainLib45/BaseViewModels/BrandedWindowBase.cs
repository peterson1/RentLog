using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using System.Windows;

namespace RentLog.DomainLib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BrandedWindowBase : UpdatedExeVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix 
            => $"{AppArgs.MarketState.SystemName} {SubAppName}";


        public abstract string SubAppName { get; }


        public BrandedWindowBase(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            AccessControlExtensions.OnUnauthorizedAccess = s => ShowNotAllowed(s);
            SetCaption($"as {AppArgs?.Credentials?.NameAndRole ?? "Anonymous"}");
        }


        protected override void ApplyWindowTheme(Window win)
        {
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //later: move xaml boilerplates to here

        }

        private static void ShowNotAllowed(string msg)
            => MessageBox.Show(msg, "  Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
