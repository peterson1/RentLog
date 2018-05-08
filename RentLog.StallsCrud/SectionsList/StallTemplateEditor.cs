using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.EnumTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
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


        public SectionDTO           Section       { get; set; }
        public LeaseDTO             Occupant      { get; set; }
        public UIList<BillInterval> BillIntervals { get; } = new UIList<BillInterval>(EnumTool.List<BillInterval>());


        protected override void SaveNewRecord(StallDTO draft)
        {
            Section.StallTemplate = draft;
            AppArgs.DB.Sections.Insert(Section);
        }


        protected override bool CanEncodeNewDraft()
            => AppArgs.CanAddSection(false);


        protected override StallDTO GetNewDraft()
        {
            if (!PopUpInput.TryGetString("Section Name", out string name)) return null;
            return GetDefaultStallTemplate(name);
        }


        private StallDTO GetDefaultStallTemplate(string sectionName) => new StallDTO
        {
            Name          = sectionName + " {0:000}",
            Section       = new SectionDTO { Name = sectionName },
            DefaultRent   = new RentParams
            {
                Interval        = BillInterval.Daily,
                GracePeriodDays = 3,
                PenaltyRule     = "Daily Surcharge",
                PenaltyRate1    = 0.03M,
            },
            DefaultRights = new RightsParams
            {
                SettlementDays = 180,
                PenaltyRule    = "Daily Surcharge after 90 days",
                PenaltyRate1   = 0.2M,
                PenaltyRate2   = 0.03M
            },
            IsOperational = true,
        };


        protected override void UpdateRecord(StallDTO record)
        {
            Section.StallTemplate = record;
            AppArgs.DB.Sections.Update(Section);
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
