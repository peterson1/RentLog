using System;
using PropertyChanged;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45;

namespace RentLog.LeasesCrud.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private AppArguments _args;


        public MainToolbarVM(AppArguments appArguments)
        {
            _args = appArguments;
        }


        public BillAmounts Overdues { get; private set; }


        public void UpdateAll()
        {
            Overdues = GetTotalOverdues();
        }


        private BillAmounts GetTotalOverdues()
        {
            var date = _args.Collections.LastPostedDate();
            return _args.Balances.TotalOverdues(date);
        }
    }
}
