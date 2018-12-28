using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLog.Cashiering.SectionTabs
{
    [AddINotifyPropertyChangedInterface]
    public class TabsListVM : UIList<SectionTabVM>
    {
        private ITenantDBsDir _dir;


        public TabsListVM(MainWindowVM mainWindowVM)
        {
            _dir = mainWindowVM.AppArgs;
        }


        public int  CurrentTabIndex  { get; private set; } = -1;


        public void OnCurrentTabIndexChanged()
            => _dir.CurrentSection = CurrentTabIndex == -1 ? null
                                   : this[CurrentTabIndex].Section;
    }
}
