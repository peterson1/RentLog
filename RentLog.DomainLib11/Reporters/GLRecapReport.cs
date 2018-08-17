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
        public GLRecapReport(Month month, int year, ITenantDBsDir tenantDBsDir)
        {
            DBsDir    = tenantDBsDir;
            StartDate = month.FirstDay(year);
            EndDate   = month.LastDay(year);

            Add(new GLRecapCategory(GLAcctType.Equity   , this));
            Add(new GLRecapCategory(GLAcctType.Asset    , this));
            Add(new GLRecapCategory(GLAcctType.Liability, this));
            Add(new GLRecapCategory(GLAcctType.Income   , this));
            Add(new GLRecapCategory(GLAcctType.Expense  , this));
        }


        public ITenantDBsDir  DBsDir     { get; }
        public DateTime       StartDate  { get; }
        public DateTime       EndDate    { get; }


        public decimal? TotalDebits  => this.Sum(_ => _.TotalDebits);
        public decimal? TotalCredits => this.Sum(_ => _.TotalCredits);
    }
}
