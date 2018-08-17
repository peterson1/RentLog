using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using System;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class GLRecapSubReport : UIList<GLRecapSubRow>
    {
        public GLRecapSubReport(GLAcctType acctTyp, ITenantDBsDir dir)
        {
            AccountType = acctTyp;
            FillWithSubRows(dir);
        }


        public GLAcctType  AccountType  { get; }


        public decimal TotalDebits  => this.Sum(_ => _.TotalDebits);
        public decimal TotalCredits => this.Sum(_ => _.TotalCredits);


        private void FillWithSubRows(ITenantDBsDir dir)
        {
            //dir.Vouchers.PassbookRows.GetRepo()
        }
    }
}
