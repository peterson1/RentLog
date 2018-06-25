using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.StringTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Models
{
    public class WithAllocations : DocumentDTOBase
    {
        public List<AccountAllocation>  Allocations  { get; set; }

        public string  DebitAccounts  => GetAccountNames(_ => _.IsDebit );
        public string  CreditAccounts => GetAccountNames(_ => _.IsCredit);
        public decimal TotalDebit     => Allocations?.Sum(_ => _.AsDebit  ?? 0) ?? 0;
        public decimal TotalCredit    => Allocations?.Sum(_ => _.AsCredit ?? 0) ?? 0;
        public bool    IsBalanced     => TotalCredit == TotalDebit;


        private string GetAccountNames(Func<AccountAllocation, bool> filter)
        {
            if (Allocations == null) return string.Empty;
            if (!Allocations.Any()) return string.Empty;
            var matches = Allocations.Where(filter);
            if (!matches.Any()) return string.Empty;
            return string.Join(L.f, matches.Select(_ => _.Account?.Name));
        }
    }
}
