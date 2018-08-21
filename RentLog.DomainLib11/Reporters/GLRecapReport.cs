using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class GLRecapReport : UIList<GLRecapCategory>
    {
        private const string LONG_FMT = "MMMM d, yyyy";


        public GLRecapReport(Month month, int year, ITenantDBsDir tenantDBsDir)
        {
            DBsDir     = tenantDBsDir;
            BranchName = DBsDir.MarketState.BranchName;
            StartDate  = month.FirstDay(year);
            EndDate    = month.LastDay(year);

            Add(new GLRecapCategory(GLAcctType.Equity   , this));
            Add(new GLRecapCategory(GLAcctType.Asset    , this));
            Add(new GLRecapCategory(GLAcctType.Liability, this));
            Add(new GLRecapCategory(GLAcctType.Income   , this));
            Add(new GLRecapCategory(GLAcctType.Expense  , this));
        }


        public string         Title       { get; set; } = "GL Recap";
        public string         BranchName  { get; }
        public ITenantDBsDir  DBsDir      { get; }
        public DateTime       StartDate   { get; }
        public DateTime       EndDate     { get; }

        public string   DateRangeText => $"{StartDate.ToString(LONG_FMT)}  to  {EndDate.ToString(LONG_FMT)}";
        public decimal? TotalDebits   => this.Sum(_ => _.TotalDebits);
        public decimal? TotalCredits  => this.Sum(_ => _.TotalCredits);
    }
}
