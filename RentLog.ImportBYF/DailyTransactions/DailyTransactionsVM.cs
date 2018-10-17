using PropertyChanged;

namespace RentLog.ImportBYF.DailyTransactions
{
    [AddINotifyPropertyChangedInterface]
    public class DailyTransactionsVM
    {
        public DailyTransactionsVM(MainWindowVM mainWindowVM)
        {
            MainWindow  = mainWindowVM;
            DateChooser = new DateChooserVM(this);
        }


        public MainWindowVM       MainWindow  { get; }
        public DateChooserVM      DateChooser { get; }
        public TransactionRowsVM  DailyRows   { get; } = new TransactionRowsVM();
    }
}
