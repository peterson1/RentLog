using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;

namespace RentLog.ChequeVouchers.AllVoucherRequests
{
    [AddINotifyPropertyChangedInterface]
    public class AllVoucherRequestsVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "All Voucher Requests";


        public AllVoucherRequestsVM(MainWindowVM mainWindowVM) : base(mainWindowVM.AppArgs)
        {
            var args = mainWindowVM.AppArgs;
            var repo = args.Vouchers.AllRequests;
            MainList = new MainListVM(repo, args, false);
            LaunchWindowCmd = R2Command.Relay(LaunchWindow, null, "View All Voucher Requests");
//#if DEBUG
//            LaunchWindowCmd.ExecuteIfItCan();
//#endif
        }


        public MainListVM  MainList         { get; }
        public IR2Command  LaunchWindowCmd  { get; }


        protected override void OnRefreshClicked()
            => MainList.ReloadFromDB();


        private void LaunchWindow()
        {
            this.Show<AllChequeVouchersWindow>();
            ClickRefresh();
        }
    }
}
