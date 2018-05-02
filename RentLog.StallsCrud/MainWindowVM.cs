using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using RentLog.DomainLib45.BaseViewModels;
using RentLog.StallsCrud.StallsList;
using System.Linq;

namespace RentLog.StallsCrud
{
    internal class MainWindowVM : BrandedWindowBase
    {
        public MainWindowVM(AppArguments args) : base(args)
        {
            Stalls = new StallsListVM(args.Stalls, args, false);
            Sections.SetItems(args.Sections.GetAll());
            CurrentSection = Sections.FirstOrDefault();
        }


        public StallsListVM        Stalls          { get; }
        public UIList<SectionDTO>  Sections        { get; } = new UIList<SectionDTO>();
        public SectionDTO          CurrentSection  { get; set; }


        protected override void OnRefreshClicked()
        {
            Stalls.Section = CurrentSection;
            Stalls.ReloadFromDB();
        }


        public void OnCurrentSectionChanged() => ClickRefresh();
    }
}
