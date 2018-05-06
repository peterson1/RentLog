using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;

namespace RentLog.StallsCrud.StallsList
{
    public class StallsFilterVM : TextFilterBase<StallRow>
    {
        public string  FilterStall             { get; set; }
        public string  FilterOccupant          { get; set; }
        public string  FilterRentRate          { get; set; }
        public string  FilterGracePeriod       { get; set; }
        public string  FilterTotalRights       { get; set; }
        public string  FilterRightsSettlement  { get; set; }
        public string  FilterIsOperational     { get; set; }


        protected override Dictionary<string, Func<StallRow, string>> FilterProperties => new Dictionary<string, Func<StallRow, string>>
        {
            { nameof(FilterStall           ), _ => _.DTO.Name },
            { nameof(FilterOccupant        ), _ => _.Occupant?.Tenant.FirstAndLastNames },
            { nameof(FilterRentRate        ), _ => _.DTO.DefaultRent.RegularRate.ToString() },
            { nameof(FilterGracePeriod     ), _ => _.DTO.DefaultRent.GracePeriodDays.ToString() },
            { nameof(FilterTotalRights     ), _ => _.DTO.DefaultRights.TotalAmount.ToString() },
            { nameof(FilterRightsSettlement), _ => _.DTO.DefaultRights.SettlementDays.ToString() },
            { nameof(FilterIsOperational   ), _ => _.DTO.IsOperational.ToString() },
        };
    }
}
