using PropertyChanged;
using RentLog.ChequeVouchers.DcdrTab.PassbookRows;
using RentLog.ChequeVouchers.DcdrTab.ReportSettings;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.PassbookRepos;

namespace RentLog.ChequeVouchers.DcdrTab
{
    [AddINotifyPropertyChangedInterface]
    public class DcdrTabVM
    {
        private MainWindowVM _main;

        public DcdrTabVM(MainWindowVM mainWindowVM)
        {
            _main        = mainWindowVM;
            DateRange    = new DateRangePickerVM();
            PassbookRows = new PassbookRowsVM(DateRange, _main.AppArgs);
        }


        public DateRangePickerVM  DateRange     { get; }
        public PassbookRowsVM     PassbookRows  { get; }
        public bool               IsVisible     { get; set; }

        public ITenantDBsDir AppArgs => _main.AppArgs;
        public IPassbookRowsRepo PassbookRepo
            => AppArgs.Passbooks.GetRepo(AppArgs.CurrentBankAcct.Id);

        //public bool IsVisible => _main.SelectedIndex == 1;
    }
}
