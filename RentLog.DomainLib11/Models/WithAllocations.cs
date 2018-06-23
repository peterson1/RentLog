using CommonTools.Lib11.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Models
{
    public class WithAllocations : DocumentDTOBase
    {
        public List<AccountAllocation>  Allocations  { get; set; }

        public decimal TotalDebit  => Allocations?.Sum(_ => _.AsDebit  ?? 0) ?? 0;
        public decimal TotalCredit => Allocations?.Sum(_ => _.AsCredit ?? 0) ?? 0;
        public bool    IsBalanced  => TotalCredit == TotalDebit;
    }
}
