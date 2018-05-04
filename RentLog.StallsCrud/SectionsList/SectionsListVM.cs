using System.Collections.Generic;
using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;

namespace RentLog.StallsCrud.SectionsList
{
    [AddINotifyPropertyChangedInterface]
    class SectionsListVM : SavedListVMBase<SectionDTO, AppArguments>
    {
        //public event EventHandler SectionChanged;


        public SectionsListVM(AppArguments appArguments) : base(appArguments.Sections, appArguments, true)
        {
        }


        //public SectionsListVM(AppArguments args)
        //{
        //    Items.SetItems(args.Sections.GetAll());
        //}

        //public UIList<SectionDTO>  Items    { get; } = new UIList<SectionDTO>();
        //public SectionDTO          Current  { get; set; }

        //public void OnCurrentChanged() 
        //    => SectionChanged?.Invoke(this, EventArgs.Empty);
    }
}
