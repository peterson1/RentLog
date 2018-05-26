using PropertyChanged;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;

namespace RentLog.Cashiering
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Cashiering";


        public MainWindowVM(AppArguments appArguments) : base(appArguments)
        {
        }
    }
}
