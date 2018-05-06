using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using RentLog.StallsCrud.StallCRUD;

namespace RentLog.StallsCrud.SectionsList
{
    public class StallTemplateEditor : CrudWindowVMBase<StallDTO, StallCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Stall Template";
        protected override string CaptionPrefix   => "Stall Template";


        public StallTemplateEditor(AppArguments appArguments) : base(appArguments)
        {
        }


        public SectionDTO Section { get; set; }


        protected override void SaveNewRecord(StallDTO draft)
        {
            Section.StallTemplate = draft;
            AppArgs.Sections.Insert(Section);
        }


        protected override void UpdateRecord(StallDTO record)
        {
            Section.StallTemplate = record;
            AppArgs.Sections.Update(Section);
        }


        protected override bool IsValidDraft(StallDTO draft, out string whyInvalid)
        {
            if (draft.Name.IsBlank())
            {
                whyInvalid = "Stall name should not be blank.";
                return false;
            }
            whyInvalid = "";
            return true;
        }
    }
}
