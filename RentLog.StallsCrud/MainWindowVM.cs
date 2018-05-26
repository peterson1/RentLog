using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.StallsCrud.SectionsList;
using RentLog.StallsCrud.StallsList;
using System.Linq;

namespace RentLog.StallsCrud
{
    [AddINotifyPropertyChangedInterface]
    internal class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Stalls";


        public MainWindowVM(AppArguments args) : base(args)
        {
            Sections = new SectionsListVM(args);
            Stalls   = new StallsListVM(args);

            Sections.SelectedChanged += (s, e) => ClickRefresh();
            Sections.Selected = Sections.ItemsList.FirstOrDefault();
        }


        public SectionsListVM  Sections  { get; }
        public StallsListVM    Stalls    { get; }


        protected override void OnRefreshClicked()
        {
            if (Sections.Selected == null)
                Sections.Selected = Sections.ItemsList.FirstOrDefault();

            if (Sections.Selected == null) return;

            AppArgs.CurrentSection = Sections.Selected;
            Stalls.ReloadFromDB();
        }
    }
}
