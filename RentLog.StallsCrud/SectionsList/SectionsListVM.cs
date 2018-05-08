using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System.Linq;

namespace RentLog.StallsCrud.SectionsList
{
    [AddINotifyPropertyChangedInterface]
    class SectionsListVM : SavedListVMBase<SectionDTO, AppArguments>
    {
        public SectionsListVM(AppArguments appArguments) : base(appArguments.MarketState.Sections, appArguments, true)
        {
            Crud           = new StallTemplateEditor(appArguments);
            EditCurrentCmd = R2Command.Relay(EditCurrent, _ => CanEditRecord(AppArgs.CurrentSection), "Edit this Section");
        }


        public StallTemplateEditor Crud            { get; }
        public IR2Command          EditCurrentCmd  { get; }


        protected override bool CanEditRecord(SectionDTO rec)
            => AppArgs.CanEditSection(false);


        private void EditCurrent()
        {
            if (!PopUpInput.TryGetString("Section Name", out string name, Selected.Name)) return;
            Selected.Name = name;
            Crud.Section = Selected;
            Crud.EditCurrentRecord(Selected.StallTemplate);
        }
    }
}
