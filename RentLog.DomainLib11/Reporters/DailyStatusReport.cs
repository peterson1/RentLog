using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyStatusReport
    {
        public DailyStatusReport(DateTime date, ITenantDBsDir tenantDBsDir)
        {
            Date            = date;
            var colxnsDb    = tenantDBsDir.Collections.For(date);
            var mkt         = tenantDBsDir.MarketState;
            SectionColxns   = new DailyColxnsReport(date, tenantDBsDir);
            StallsInventory = new StallsInventoryReport(colxnsDb, mkt);
            CollectorsPerf  = new CollectorsPerformanceReport(date, tenantDBsDir);
            OtherColxns     = LoadOtherColxns(colxnsDb);
            BankDeposits    = LoadBankDeposits(colxnsDb);
        }


        public DateTime                     Date             { get; }
        public DailyColxnsReport            SectionColxns    { get; }
        public StallsInventoryReport        StallsInventory  { get; }
        public CollectorsPerformanceReport  CollectorsPerf   { get; }
        public UIList<OtherColxnDTO>        OtherColxns      { get; }
        public UIList<BankDepositDTO>       BankDeposits     { get; }


        private UIList<OtherColxnDTO> LoadOtherColxns(ICollectionsDB db)
        {
            var all  = db.OtherColxns.GetAll();
            var list = new UIList<OtherColxnDTO>(all);
            list.SetSummary(new OtherColxnDTO
            {
                Amount = all.Sum(_ => _.Amount)
            });
            return list;
        }


        private UIList<BankDepositDTO> LoadBankDeposits(ICollectionsDB db)
        {
            var all  = db.BankDeposits.GetAll();
            var list = new UIList<BankDepositDTO>(all);
            list.SetSummary(new BankDepositDTO
            {
                Amount = all.Sum(_ => _.Amount)
            });
            return list;
        }
    }
}
