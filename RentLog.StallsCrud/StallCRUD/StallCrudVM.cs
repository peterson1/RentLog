using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib45;
using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.Models;
using CommonTools.Lib11.EnumTools;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Linq;

namespace RentLog.StallsCrud.StallCRUD
{
    public class StallCrudVM : RepoCrudWindowVMBase<IStallsRepo, StallDTO, StallCrudWindow, AppArguments>
    {
        public    override string TypeDescription => "Stall";
        protected override string CaptionPrefix   => "Stall";


        public StallCrudVM(AppArguments appArguments) : base(appArguments.MarketState.Stalls, appArguments)
        {
        }


        public LeaseDTO Occupant { get; set; }
        public UIList<BillInterval> BillIntervals { get; } = new UIList<BillInterval>(EnumTool.List<BillInterval>());


        public override bool CanEncodeNewDraft() => AppArgs.CanAddStall(false);


        protected override StallDTO GetNewDraft()
        {
            var draft     = AppArgs.CurrentSection.StallTemplate.ShallowClone<StallDTO>();
            draft.Section = AppArgs.CurrentSection;
            draft.Name    = GetSuggestedName(draft.Name);
            Occupant      = null;
            return draft;
        }


        private string GetSuggestedName(string format)
        {
            var nums = _repo.ForSection(AppArgs.CurrentSection)
                            .Select (_ => GetNumberPart(_.Name))
                            .Where  (_ => _.HasValue);
            var next = nums.Max().Value + 1;
            return string.Format(format, next);
        }


        private int? GetNumberPart(string name)
        {
            if (!name.Contains(" ")) return null;
            var ss = name.Split(' ');
            return int.TryParse(ss.Last(), out int num)
                ? num : (int?)null;
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
