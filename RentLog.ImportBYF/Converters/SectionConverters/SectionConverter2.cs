using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters.SectionConverters
{
    public class SectionConverter2 : ConverterRowBase<SectionDTO>
    {
        public override string Label          => "Sections";
        public override string ViewsDisplayID => "sections_list?display_id=page_3";

        public SectionConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override SectionDTO CastToRNT(dynamic byf)
        {
            var nme = As.Text(byf.label);
            return new SectionDTO
            {
                Id            = As.ID(byf.nid),
                Name          = nme,
                IsActive      = true,
                StallTemplate = StallDTO.Named(nme + " {0:000}"),
            };
        }


        protected override IR2Command CreateRemediate1Cmd()
            => R2Command.Relay(SetStallTemplates);


        private void SetStallTemplates()
        {
            var repo = Main.AppArgs.MarketState.Sections;
            foreach (var sec in repo.GetAll())
            {
                var tpl         = sec.StallTemplate;
                tpl.Section     = sec;
                tpl.DefaultRent = new RentParams
                {
                    Interval        = BillInterval.Daily,
                    GracePeriodDays = 3,
                    PenaltyRule     = "Daily Surcharge",
                    PenaltyRate1    = 0.03M,
                };
                tpl.DefaultRights = new RightsParams
                {
                    SettlementDays = 180,
                    PenaltyRule    = "Daily Surcharge after 90 days",
                    PenaltyRate1   = 0.2M,
                    PenaltyRate2   = 0.03M
                };
                tpl.IsOperational = true;
                repo.Update(sec);
            }
        }


        public override List<SectionDTO> GetRntRecords(ITenantDBsDir dir)
            => dir.MarketState.Sections.GetAll();


        public override void ReplaceAll(IEnumerable<SectionDTO> newRecords, MarketStateDB mkt)
            => mkt.Sections.DropAndInsert(newRecords, true, false);
    }
}
