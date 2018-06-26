using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.ExceptionTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.UIExtensions;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RentLog.DomainLib45.StallPicker
{
    [AddINotifyPropertyChangedInterface]
    internal class StallPickerVM
    {
        private Window        _win;
        private MarketStateDB _mkt;
        private List<int>     _occupiedIDs;

        internal StallPickerVM(MarketStateDB marketStateDB, Window window)
        {
            _mkt = marketStateDB;
            _win = window;
            if (_win != null)
                _win.DataContext = this;
            _occupiedIDs = GetOccupiedStallIDs();
            Sections.SetItems(marketStateDB.Sections.GetAll());
            ContinueCmd = R2Command.Relay(DoContinue, _ => CanContinue(), "Continue");
        }


        public UIList<SectionDTO> Sections      { get; } = new UIList<SectionDTO>();
        public UIList<StallDTO>   Stalls        { get; } = new UIList<StallDTO>();
        public SectionDTO         PickedSection { get; set; }
        public StallDTO           PickedStall   { get; set; }
        public IR2Command         ContinueCmd   { get; }


        public void OnPickedSectionChanged()
            => Stalls.SetItems(GetVacantStalls());


        private IEnumerable<StallDTO> GetVacantStalls()
            => _mkt.Stalls.ForSection(PickedSection)
                    .Where(_ => !_occupiedIDs.Contains(_.Id));


        private List<int> GetOccupiedStallIDs()
            => _mkt.ActiveLeases.GetAll().Select(_ => _.Stall.Id).ToList();


        private bool CanContinue() => PickedStall != null;
        private void DoContinue () => _win.DialogResult = true;
    }


    public class StallPicker
    {
        public static bool TryPick(MarketStateDB marketStateDB, out StallDTO stall)
        {
            var win = CreateWindow();
            var vm  = new StallPickerVM(marketStateDB, win);
            var res = win.ShowDialog();
            stall   = vm.PickedStall;
            return res == true && stall != null;
        }


        public static StallDTO PickFirstVacant(MarketStateDB mkt)
        {
            var vm = new StallPickerVM(mkt, null);
            foreach (var sec in vm.Sections)
            {
                vm.PickedSection = sec;
                if (vm.Stalls.Any())
                    return vm.Stalls.First();
            }
            throw No.Match<StallDTO>("state", "vacant");
        }


        private static StallPickerWindow CreateWindow()
        {
            var win = new StallPickerWindow();
            win.MakeDraggable();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.SnapsToDevicePixels = true;
            win.SetToCloseOnEscape();
            return win;
        }
    }
}
