using PropertyChanged;
using RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList;

namespace RentLog.ImportBYF.Version2UI.TransactionDataPane
{
    [AddINotifyPropertyChangedInterface]
    public class TransactionDataPaneVM
    {
        public TransactionDataPaneVM(MainWindowVM2 main)
        {
            PeriodsList = new PeriodsListVM(main);

            main.ByfServer.GotMinMaxDates
                += (s, e) => PeriodsList.FillPeriodsList(e);
        }


        public PeriodsListVM  PeriodsList  { get; }
    }
}
