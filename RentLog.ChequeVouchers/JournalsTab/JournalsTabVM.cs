using RentLog.ChequeVouchers.JournalsTab.JournalsList;

namespace RentLog.ChequeVouchers.JournalsTab
{
    public class JournalsTabVM
    {
        public JournalsTabVM(MainWindowVM main)
        {
            JournalRows = new JournalsListVM(main);
        }


        public JournalsListVM JournalRows { get; }


    }
}
