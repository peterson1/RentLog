using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DomainLib45.CollectorsCRUD.MainToolbar
{
    [AddINotifyPropertyChangedInterface]
    public class MainToolbarVM
    {
        private CollectorsMainVM _main;


        public MainToolbarVM(CollectorsMainVM collectorsMainVM)
        {
            _main = collectorsMainVM;
        }


        public string Header => $"Collectors ({UIList.Count})";

        private UIList<CollectorDTO> UIList => _main.Rows.ItemsList;
    }
}
