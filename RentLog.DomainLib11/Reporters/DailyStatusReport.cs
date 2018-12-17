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
        private DailyStatusReport(ITenantDBsDir dir, DateTime date)
        {
            DBsDir = dir;
            Date   = date;
        }


        public ITenantDBsDir                DBsDir           { get; }
        public DateTime                     Date             { get; }
        public string                       BranchName       { get; private set; }
        public DailyColxnsReport            SectionColxns    { get; private set; }
        public StallsInventoryReport        StallsInventory  { get; private set; }
        public CollectorsPerformanceReport  CollectorsPerf   { get; private set; }
        public UIList<OtherColxnDTO>        OtherColxns      { get; private set; }
        public UIList<BankDepositDTO>       BankDeposits     { get; private set; }
        public BillAmounts                  Overdues         { get; private set; }


        public static DailyStatusReport New (ITenantDBsDir dir, DateTime date)
        {
            var ds             = new DailyStatusReport(dir, date);
            var colxnsDb       = ds.DBsDir.Collections.For(date);
            var mkt            = ds.DBsDir.MarketState;
            ds.BranchName      = mkt.BranchName;
            ds.SectionColxns   = new DailyColxnsReport(date, ds.DBsDir);
            ds.StallsInventory = new StallsInventoryReport(colxnsDb, mkt);
            ds.CollectorsPerf  = CollectorsPerformanceReport.New(ds.DBsDir.MarketState, colxnsDb);
            ds.OtherColxns     = ds.LoadOtherColxns(colxnsDb);
            ds.BankDeposits    = ds.LoadBankDeposits(colxnsDb);
            ds.Overdues        = ds.DBsDir.Balances.TotalOverdues(date);
            return ds;
        }


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
