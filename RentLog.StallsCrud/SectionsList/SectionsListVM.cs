using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.StallsCrud.SectionsList
{
    [AddINotifyPropertyChangedInterface]
    class SectionsListVM : SavedListVMBase<SectionDTO, AppArguments>
    {
        public SectionsListVM(AppArguments appArguments) : base(appArguments.Sections, appArguments, true)
        {
            Crud           = new StallTemplateEditor(appArguments);
            EditCurrentCmd = R2Command.Relay(EditCurrent, null, "Edit this Section");
        }
        //todo:access control

        public StallTemplateEditor Crud            { get; }
        public IR2Command          EditCurrentCmd  { get; }


        //private void AddNewSection()
        //{
        //    if (!PopUpInput.TryGetString("Section Name", out string name)) return;
        //}


        private void EditCurrent()
        {
            if (!PopUpInput.TryGetString("Section Name", out string name, Selected.Name)) return;
            Selected.Name = name;
            Crud.Section = Selected;
            Crud.EditCurrentRecord(Selected.StallTemplate);
        }
    }
}
