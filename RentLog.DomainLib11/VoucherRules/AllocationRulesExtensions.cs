﻿using RentLog.DomainLib11.DTOs;
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
                => _.Account.Id == 0 //todo: less than 0?
                && _.Account.Name.Contains("Cash in Bank"));


        public static AccountAllocation AddCashInBankEntry(this List<AccountAllocation> list, BankAccountDTO bankAccount, decimal amount)
        {
            var alloc = AccountAllocation.DefaultCashInBank(bankAccount, amount);
            list.Insert(0, alloc);
            return alloc;
        }
    }
}
