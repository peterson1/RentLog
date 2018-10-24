﻿using CommonTools.Lib11.DynamicTools;
using CommonTools.Lib11.StringTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.ImportBYF.Converters.LeaseRevisionConverters;
using RentLog.ImportBYF.Converters.MiscellaneousConverters;
using RentLog.ImportBYF.Converters.TenantConverters;
using RentLog.ImportBYF.Version2UI;
using RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.ImportBYF.Converters.LeaseConverters
{
    public partial class LeaseConverter2 : ConverterRowBase<LeaseDTO>
    {
        public override string Label          => "Leases";
        public override string ViewsDisplayID => "leases_list?display_id=page_2";

        private Dictionary<int, TenantModel> _tenantsDict;
        private Dictionary<int, StallDTO>    _stallsDict;
        private Dictionary<int, DateTime>    _terminationDates;


        public LeaseConverter2(MainWindowVM2 mainWindowVM2) : base(mainWindowVM2)
        {
        }


        public override LeaseDTO CastToRNT(dynamic byf)
        {
            var period    = GetContractDates((string)byf.contractstartend);
            var tenant    = _tenantsDict[As.ID(byf.tenantnid)];
            var stall     = _stallsDict[As.ID(byf.stallnid)];
            var rentPrm   = CastRentParams(byf, period);
            var rightsPrm = CastRightsParams(byf, period);
            var rntLse    = new LeaseDTO
            {
                Id                   = As.ID(byf.nid),
                Tenant               = tenant,
                Stall                = stall,
                ContractStart        = period.Start,
                ContractEnd          = period.End,
                ProductToSell        = As.Text(byf.producttosell),
                Rent                 = rentPrm,
                Rights               = rightsPrm,
                ApplicationSubmitted = As.Date(byf.formsubmitteddate),
                Remarks              = As.Text(byf.remarks),
            };
            return rntLse.AsInactiveIfSo(_terminationDates, Main.ByfServer.LastPostedDate);
        }


        public override async Task BeforeByfQuery()
        {
            _stallsDict       = Main.AppArgs.MarketState.Stalls
                                    .GetAll().ToDictionary(_ => _.Id);
            _tenantsDict      = await this.GetTenantsDictionary();
            _terminationDates = await this.GetTerminationDates();
        }

        private (DateTime Start, DateTime End) GetContractDates(string periodText)
        {
            var split = periodText.SplitTrim("to");
            return (DateTime.Parse(split[0]).ToLocalTime(),
                    DateTime.Parse(split[1]).ToLocalTime());
        }


        public override List<LeaseDTO> GetRntRecords(ITenantDBsDir dir) 
            => dir.MarketState.GetAllLeases();


        public override void ReplaceAll(IEnumerable<LeaseDTO> newRecords, MarketStateDB mkt)
        {
            var actives = newRecords.Where(_ => !(_ is InactiveLeaseDTO));
            var inactvs = newRecords.Where(_ =>   _ is InactiveLeaseDTO)
                                    .Select(_ => _ as InactiveLeaseDTO);

            mkt.ActiveLeases  .DropAndInsert(actives, true, false);
            mkt.InactiveLeases.DropAndInsert(inactvs, true, false);
        }
    }
}