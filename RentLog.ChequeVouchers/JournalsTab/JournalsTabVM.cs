using PropertyChanged;
using RentLog.ChequeVouchers.JournalsTab.JournalsList;

namespace RentLog.ChequeVouchers.JournalsTab
{
    [AddINotifyPropertyChangedInterface]
    public class JournalsTabVM
    {
        public JournalsTabVM(MainWindowVM main)
        {
            JournalRows = new JournalsListVM(main);
        }


        public JournalsListVM JournalRows { get; }


    }
}
