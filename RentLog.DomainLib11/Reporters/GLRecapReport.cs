using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DateTimeTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class GLRecapReport : UIList<GLRecapSubReport>
    {
        public GLRecapReport(Month month, int year, ITenantDBsDir dir)
        {
            StartDate = month.FirstDay(year);
            EndDate   = month.LastDay(year);

            Add(new GLRecapSubReport(GLAcctType.Equity   , dir));
            Add(new GLRecapSubReport(GLAcctType.Asset    , dir));
            Add(new GLRecapSubReport(GLAcctType.Liability, dir));
            Add(new GLRecapSubReport(GLAcctType.Income   , dir));
            Add(new GLRecapSubReport(GLAcctType.Expense  , dir));
        }


        public DateTime  StartDate  { get; }
        public DateTime  EndDate    { get; }


        public decimal TotalDebits  => this.Sum(_ => _.TotalDebits);
        public decimal TotalCredits => this.Sum(_ => _.TotalCredits);
    }
}
