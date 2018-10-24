using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.TransactionDataPane.PeriodsList
{
    [AddINotifyPropertyChangedInterface]
    public class PeriodsListVM : UIList<PeriodRowVM>
    {
        public PeriodsListVM(MainWindowVM2 mainWindowVM2)
        {
            MainWindow = mainWindowVM2;
        }


        public MainWindowVM2 MainWindow { get; }


        public void FillPeriodsList((DateTime Min, DateTime Max) periods)
        {
            var list = periods.Min.EachDayUpTo(periods.Max)
                              .Select(_ => new PeriodRowVM(_, MainWindow));
            UIThread.Run(() => SetItems(list));
        }
    }
}
