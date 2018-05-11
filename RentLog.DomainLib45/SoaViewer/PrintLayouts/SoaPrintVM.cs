using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.PrintTools;
using RentLog.DomainLib45.SoaViewer.MainWindow;

namespace RentLog.DomainLib45.SoaViewer.PrintLayouts
{
    public class SoaPrintVM : MainWindowVMBase<AppArguments>
    {
        protected override string CaptionPrefix => "Statement of Account";


        public SoaPrintVM(SoaViewerVM soaViewerVM) : base(soaViewerVM.AppArgs)
        {
            VM = soaViewerVM;
        }


        public SoaViewerVM VM { get; }


        public static void Print(SoaViewerVM soaViewerVM)
        {
            var vm          = new SoaPrintVM(soaViewerVM);
            var win         = new SoaPrintWindow1();
            win.DataContext = vm;
            var lse         = soaViewerVM.Lease;
            win.dg.AskToPrint(lse.TenantAndStall,
                            $"Total Rights {lse.Rights.TotalAmount:N2}  (due: {lse.RightsDueDate:d MMM yyyy})",
                            $"as of {vm.AppArgs.Collections.LastPostedDate():MMMM d, yyyy}",
                            $":  {vm.AppArgs.MarketState.BranchName}");
            win.Close();
        }
    }
}
