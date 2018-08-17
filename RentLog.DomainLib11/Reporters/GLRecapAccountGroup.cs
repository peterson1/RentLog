using CommonTools.Lib11.DataStructures;
using RentLog.DomainLib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Reporters
{
    public class GLRecapAccountGroup : UIList<GLRecapAllocation>
    {
        public GLRecapAccountGroup(GLAccountDTO glAccountDTO, IEnumerable<GLRecapAllocation> items) : base(items)
        {
            Account = glAccountDTO;
        }


        public GLAccountDTO Account { get; }


        public decimal? TotalDebits  => this.Sum(_ => _.AsDebit);
        public decimal? TotalCredits => this.Sum(_ => _.AsCredit);
    }
}
