using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;

namespace RentLog.LeasesCrud.LeasesList
{
    public class LeasesFilterVM : TextFilterBase<LeaseBalanceRow>
    {
        public string  TenantFilter          { get; set; }
        public string  StallFilter           { get; set; }
        public string  ContractStartFilter   { get; set; }
        public string  ContractEndFilter     { get; set; }
        public string  DeactivatedDateFilter { get; set; }
        public string  DeactivatedByFilter   { get; set; }
        public string  WhyInactiveFilter     { get; set; }
        public string  RentIntervalFilter    { get; set; }
        public string  RentRateFilter        { get; set; }
        public string  TotalRightsFilter     { get; set; }
        public string  RightsDueFilter       { get; set; }
        public string  RightsDateFilter      { get; set; }
        public string  ProductToSellFilter   { get; set; }
        public string  RemarksFilter         { get; set; }


        protected override Dictionary<string, Func<LeaseBalanceRow, string>> FilterProperties => new Dictionary<string, Func<LeaseBalanceRow, string>>
        {
                { nameof(TenantFilter         ), _ => _.DTO.Tenant.LastAndFirstNames            },
                { nameof(StallFilter          ), _ => _.DTO.Stall.Name                          },
                { nameof(ContractStartFilter  ), _ => _.DTO.ContractStart.ToString("d MMM yyyy")},
                { nameof(ContractEndFilter    ), _ => _.DTO.ContractEnd?.ToString("d MMM yyyy")  },
                { nameof(DeactivatedDateFilter), _ => _.DeactivatedDate?.ToString("d MMM yyyy") },
                { nameof(DeactivatedByFilter  ), _ => _.DeactivatedBy                           },
                { nameof(WhyInactiveFilter    ), _ => _.WhyInactive                             },
                { nameof(RentIntervalFilter   ), _ => _.DTO.Rent.Interval.ToString()            },
                { nameof(RentRateFilter       ), _ => _.DTO.Rent.RegularRate.ToString()         },
                { nameof(TotalRightsFilter    ), _ => _.DTO.Rights.TotalAmount.ToString()       },
                { nameof(RightsDueFilter      ), _ => _.DTO.Rights.SettlementDays.ToString()    },
                { nameof(RightsDateFilter     ), _ => _.DTO.RightsDueDate.ToString("d MMM yyyy")},
                { nameof(ProductToSellFilter  ), _ => _.DTO.ProductToSell                       },
                { nameof(RemarksFilter        ), _ => _.DTO.Remarks                             },
        };
    }
}
