using RentLog.DomainLib11.BillingRules.RentPenalties;
using RentLog.DomainLib11.BillingRules.RightsPenalties;
using RentLog.DomainLib11.Models;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public static class RentRightsParamsConverter1
    {


        public static RentParams CastRentParams(this ReportModels.Lease byf) => new RentParams
        {
            GracePeriodDays = (byf.Rent.FirstDueDate - byf.ContractStart).Days,
            Interval        = BillInterval.Daily,
            RegularRate     = (decimal)byf.Rent.RegularRate,
            PenaltyRule     = RentPenalty.DailySurcharge,
            PenaltyRate1    = (decimal)byf.Rent.PenaltyRate1.Item,
            PenaltyRate2    = (decimal)byf.Rent.PenaltyRate2.Item,
        };


        public static RightsParams CastRightsParams(this ReportModels.Lease byf) => new RightsParams
        {
            SettlementDays = (byf.Rights.DueDate - byf.ContractStart).Days,
            TotalAmount    = (decimal)byf.Rights.TotalAmount,
            PenaltyRule    = RightsPenalty.DailyAfter90Days,
            PenaltyRate1   = (decimal)byf.Rights.PenaltyRate1.Item,
            PenaltyRate2   = (decimal)byf.Rights.PenaltyRate2.Item,
        };

    }
}
