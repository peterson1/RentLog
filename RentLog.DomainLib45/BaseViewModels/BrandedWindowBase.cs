using CommonTools.Lib45.ApplicationTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.GoogleTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DataSources;
using System;
using System.Windows;

namespace RentLog.DomainLib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BrandedWindowBase : UpdatedExeVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix 
            => $"{AppArgs?.MarketState?.BranchName}  :  {AppArgs?.MarketState?.SystemName} {SubAppName}";


        public abstract string SubAppName { get; }


        public BrandedWindowBase(ITenantDBsDir tenantDBsDir) : base(tenantDBsDir)
        {
            AccessControlExtensions.OnUnauthorizedAccess = s => ShowNotAllowed(s);

            AppInitializer   .OnError = (e, c) => OnGlobalError(e, c);
            R2AsyncCommandWPF.OnError = (e, c) => OnGlobalError(e, c);

            SetCaption($"as {AppArgs?.Credentials?.NameAndRole ?? "Anonymous"}");
        }


        private async void OnGlobalError(Exception exception, string context)
        {
            Alert.Show(exception, context, false, false);
            await FirebaseSender.Post(exception, context, AppArgs.Credentials);
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
