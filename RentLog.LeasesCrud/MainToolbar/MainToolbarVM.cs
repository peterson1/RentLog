using CommonTools.Lib11.InputCommands;
using PropertyChanged;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45;
using RentLog.DomainLib45.WithOverduesReport;

namespace RentLog.LeasesCrud.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private AppArguments _args;


        public MainToolbarVM(AppArguments appArguments)
        {
            _args = appArguments;
            WithOverduesReportCmd = WithOverduesReport.CreateLauncherCmd(_args);
        }


        public IR2Command   WithOverduesReportCmd  { get; }
        public BillAmounts  Overdues               { get; private set; }


        public void UpdateAll()
        {
            Overdues = _args.Balances.TotalOverdues();
        }
    }
}
