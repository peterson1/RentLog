using CommonTools.Lib11.DynamicTools;
using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.BillingRules.RightsPenalties;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public partial class LeaseConverter2 : ConverterRowBase<LeaseDTO>
    {
        protected RentParams CastRentParams(dynamic byf, (DateTime Start, DateTime End) period)
        {
            var firstRentDue = As.Date(byf.firstrentduedate);
            var gracePDays   = ((DateTime)firstRentDue - period.Start).Days;
            return new RentParams
            {
                GracePeriodDays = gracePDays,
                Interval        = BillInterval.Daily,
                RegularRate     = As.Decimal(byf.rentrate),
                PenaltyRule     = RentPenalty.DailySurcharge,
                PenaltyRate1    = As.Decimal_(byf.rentsurchargerate) ?? 0.03M,
                PenaltyRate2    = 0.0M,
            };
        }


        protected RightsParams CastRightsParams(dynamic byf, (DateTime Start, DateTime End) period)
        {
            var dueDate = (DateTime)As.Date(byf.rightsduedate);
            return new RightsParams
            {
                SettlementDays = (dueDate - period.Start).Days,
                TotalAmount    = As.Decimal(byf.totalrights),
                PenaltyRule    = RightsPenalty.DailyAfter90Days,
                PenaltyRate1   = As.Decimal_(byf.rightssurchargerate) ?? 0.20M,
                PenaltyRate2   = 0.03M,
            };
        }
    }
}
