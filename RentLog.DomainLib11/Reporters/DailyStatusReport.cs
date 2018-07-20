using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class DailyStatusReport
    {
        public DailyStatusReport(DateTime date, ITenantDBsDir dir)
        {
            Date            = date;
            var colxnsDb    = dir.Collections.For(date);
            var mkt         = dir.MarketState;
            BranchName      = mkt.BranchName;
            SectionColxns   = new DailyColxnsReport(date, dir);
            StallsInventory = new StallsInventoryReport(colxnsDb, mkt);
            CollectorsPerf  = new CollectorsPerformanceReport(colxnsDb, mkt);
            OtherColxns     = LoadOtherColxns(colxnsDb);
            BankDeposits    = LoadBankDeposits(colxnsDb);
            Overdues        = dir.Balances.TotalOverdues(date);

            //SectionColxns[0].Total
            //OtherColxns[0].GLAccount
            //CollectorsPerf[0].Sections
        }


        public DateTime                     Date             { get; }
        public string                       BranchName       { get; }
        public DailyColxnsReport            SectionColxns    { get; }
        public StallsInventoryReport        StallsInventory  { get; }
        public CollectorsPerformanceReport  CollectorsPerf   { get; }
        public UIList<OtherColxnDTO>        OtherColxns      { get; }
        public UIList<BankDepositDTO>       BankDeposits     { get; }
        public BillAmounts                  Overdues         { get; }


        public decimal? CollectionsTotal => SectionColxns?.SectionsTotal
                                          + (decimal)OtherColxns?.SummaryAmount;


        private UIList<OtherColxnDTO> LoadOtherColxns(ICollectionsDB db)
        {
            var all  = db.OtherColxns.GetAll();
            var list = new UIList<OtherColxnDTO>(all);
            list.SetSummary(new OtherColxnDTO
            {
                Amount = all.Sum(_ => _.Amount)
            });
            list.SummaryAmount = (double)list.SummaryRows[0].Amount;
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
            list.SummaryAmount = (double)list.SummaryRows[0].Amount;
            return list;
        }
    }
}
