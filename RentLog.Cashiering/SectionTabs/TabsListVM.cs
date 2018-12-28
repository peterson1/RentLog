using CommonTools.Lib11.DataStructures;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.Cashiering.SectionTabs
{
    [AddINotifyPropertyChangedInterface]
    public class TabsListVM : UIList<SectionTabVM>
    {
        //private ITenantDBsDir _dir;


        public TabsListVM(MainWindowVM mainWindowVM)
        {
            //_dir = mainWindowVM.AppArgs;
            Main = mainWindowVM;
        }


        public int           CurrentTabIndex  { get; private set; } = -1;
        public MainWindowVM  Main             { get; }


        public SectionTabVM Current => GetCurrentTab();

        private SectionTabVM GetCurrentTab()
        {
            if (!this.Any()) return null;
            if (CurrentTabIndex < 0) return null;
            return this[CurrentTabIndex];
        }


        public void FillTabs()
        {
            var mkt    = Main.AppArgs.MarketState;
            var list   = new List<SectionTabVM>();
            var all    = mkt.Sections.GetAll();
            var activs = mkt.ActiveLeasesFor(Main.Date);

            foreach (var sec in all)
            {
                if (activs.Any(_ => _.Stall.Section.Id == sec.Id))
                    list.Add(new SectionTabVM(list.Count, sec, Main));
            }

            UIThread.Run(() => this.SetItems(list));
            Main.AppArgs.CurrentSection = this.FirstOrDefault()?.Section;
        }


        public void OnCurrentTabIndexChanged()
            => Main.AppArgs.CurrentSection = CurrentTabIndex == -1 ? null
                                           : this[CurrentTabIndex].Section;
    }
}
