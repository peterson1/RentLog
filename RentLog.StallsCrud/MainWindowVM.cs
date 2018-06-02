using PropertyChanged;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.StallsCrud.SectionsList;
using RentLog.StallsCrud.StallsList;
using System.Linq;

namespace RentLog.StallsCrud
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : BrandedWindowBase
    {
        public override string SubAppName => "Stalls";


        public MainWindowVM(AppArguments args) : base(args)
        {
            Sections = new SectionsListVM(args);
            Stalls   = new StallsListVM(this, args);

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
