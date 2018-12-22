using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Version2UI;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalAdjustmentsVM : MainWindowVMBase<ITenantDBsDir>
    {
        protected override string CaptionPrefix => "Lease Bal Adj Verifier";


        public LeaseBalAdjustmentsVM(LeaseDTO lease, MainWindowVM2 mainWindowVM2) : base(mainWindowVM2.AppArgs)
        {
            Lease     = lease;
            Main      = mainWindowVM2;
            Rows      = new LeaseBalAdjsList();
            ImportCmd = R2Command.Async(_ => Rows.Import(this), null, "Import");
            SetCaption($"{Lease}");
            ClickRefresh();
        }


        public MainWindowVM2     Main       { get; }
        public LeaseDTO          Lease      { get; }
        public LeaseBalAdjsList  Rows       { get; }
        public IR2Command        ImportCmd  { get; }


        protected override async Task OnRefreshClickedAsync()
        {
            await Rows.Fill(this);
            if (!Rows.Any()) this.CloseWindow();
            if (Rows.All(_ => _.AreEqual)) this.CloseWindow();
            //if (CanAutoImport()) ImportCmd.ExecuteIfItCan();
        }


        private bool CanAutoImport()
        {
            if (Rows.All(_ => _.RntDTO == null)) return true;
            return false;
        }
    }
}
