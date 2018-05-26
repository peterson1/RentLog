using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.Cashiering.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private MainWindowVM _main;

        public MainToolbarVM(MainWindowVM mainWindowVM)
        {
            _main = mainWindowVM;
        }
    }
}
