using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.VoucherRules
{
    public static class AllocationRulesExtensions
    {
        public static bool HasCashInBankEntry(this FundRequestDTO req)
            => req?.Allocations?.HasCashInBankEntry() ?? false;


        public static bool HasCashInBankEntry(this List<AccountAllocation> allocations)
            => allocations?.GetCashInBankEntry() != null;


        public static AccountAllocation GetCashInBankEntry(this List<AccountAllocation> allocations)
            => allocations?.FirstOrDefault(_
                => _.Account.Id == 0
                && _.Account.Name.Contains("Cash in Bank")
                && _.IsCredit);
    }
}
