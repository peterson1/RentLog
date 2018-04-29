using CommonTools.Lib45.BaseViewModels;
using System.Windows;

namespace RentLog.DomainLib45.BaseViewModels
{
    public abstract class BrandedWindowBase : UpdatedExeVMBase<AppArguments>
    {
        protected override string CaptionPrefix => AppArgs.SystemName;


        public BrandedWindowBase(AppArguments appArguments) : base(appArguments)
        {
        }


        protected override void ApplyWindowTheme(Window win)
        {
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //later: move xaml boilerplates to here

        }
    }
}
