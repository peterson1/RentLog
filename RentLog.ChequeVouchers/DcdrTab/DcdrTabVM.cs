using PropertyChanged;
using RentLog.ChequeVouchers.DcdrTab.PassbookRows;
using RentLog.DomainLib11.DataSources;
using System.Threading.Tasks;

namespace RentLog.ChequeVouchers.DcdrTab
{
    [AddINotifyPropertyChangedInterface]
    public class DcdrTabVM
    {
        private MainWindowVM _main;


        public DcdrTabVM(MainWindowVM mainWindowVM)
        {
            _main        = mainWindowVM;
            PassbookRows = new PassbookRowsVM(_main);
            PrintTrigger = new DcdrPrintTrigger(_main);
        }


        public PassbookRowsVM     PassbookRows  { get; }
        public DcdrPrintTrigger   PrintTrigger  { get; }

        public ITenantDBsDir AppArgs => _main.AppArgs;


        public async Task RecomputeBalances()
        {
            var date = _main.DateRange.Start;
            _main.StartBeingBusy($"Recomputing balances since [{date:d-MMM-yyyy}] ...");
            var arg  = _main.AppArgs;
            var repo = arg.Passbooks.GetRepo(arg.CurrentBankAcct.Id);
            await Task.Run(() => repo.RecomputeBalancesFrom(date));
            _main.StopBeingBusy();
            _main.ClickRefresh();
        }
    }
}
