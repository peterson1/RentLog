using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.ReportRows;
using System;
using System.Collections.Generic;

namespace RentLog.FilteredLeases.FilteredLists
{
    public class CommonTextFilterVM : TextFilterBase<LeaseBalanceRow>
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
        public string  StatusFilter         { get; set; }
        public string FirstNameFilter { get; set; }
        public string MiddleNameFilter { get; set; }
        public string LastNameFilter { get; set; }
        public string BirthdateFilter { get; set; }
        public string SSSFilter { get; set; }
        public string TINFilter { get; set; }
        public string SpouseFilter { get; set; }
        public string Phone1Filter { get; set; }
        public string Phone2Filter { get; set; }
        public string EmailFilter { get; set; }
        public string LotNumberFilter { get; set; }
        public string StreetFilter { get; set; }
        public string BarangayFilter { get; set; }
        public string MunicipalityFilter { get; set; }
        public string ProvinceFilter { get; set; }
        public string CompositeRemarksFilter { get; set; }


        protected override Dictionary<string, Func<LeaseBalanceRow, string>> FilterProperties => new Dictionary<string, Func<LeaseBalanceRow, string>>
        {
                { nameof(TenantFilter         ), _ => _.DTO.Tenant.FirstAndLastNames            },
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
                { nameof(StatusFilter  ), _ => _.StatusText                           },
                { nameof(FirstNameFilter ), _ => _.DTO.Tenant.FirstName                           },
                { nameof(MiddleNameFilter), _ => _.DTO.Tenant.MiddleName                           },
                { nameof(LastNameFilter), _ => _.DTO.Tenant.LastName                           },
                { nameof(BirthdateFilter), _ => _.DTO.Tenant.BirthDate.ToString("d MMM yyyy")                           },
                { nameof(SSSFilter), _ => _.DTO.Tenant.SssNumber                         },
                { nameof(TINFilter), _ => _.DTO.Tenant.TinNumber                         },
                { nameof(SpouseFilter), _ => _.DTO.Tenant.SpouseName                         },
                { nameof(Phone1Filter), _ => _.DTO.Tenant.Phone1                         },
                { nameof(Phone2Filter), _ => _.DTO.Tenant.Phone2                         },
                { nameof(EmailFilter), _ => _.DTO.Tenant.Email                        },
                { nameof(LotNumberFilter), _ => _.DTO.Tenant.LotNumber                       },
                { nameof(StreetFilter), _ => _.DTO.Tenant.StreetName                       },
                { nameof(BarangayFilter), _ => _.DTO.Tenant.Barangay                     },
                { nameof(MunicipalityFilter), _ => _.DTO.Tenant.Municipality                     },
                { nameof(ProvinceFilter), _ => _.DTO.Tenant.Province                    },
                { nameof(CompositeRemarksFilter), _ => _.CompositeRemarks                    },
        };
    }
}
