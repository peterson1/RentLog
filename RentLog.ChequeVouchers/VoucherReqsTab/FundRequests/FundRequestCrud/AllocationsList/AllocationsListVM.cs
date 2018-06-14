using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using static RentLog.DomainLib11.DTOs.FundRequestDTO;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud.AllocationsList
{
    [AddINotifyPropertyChangedInterface]
    public class AllocationsListVM : UIList<AccountAllocation>
    {
        private List<AccountAllocation> _list;
        private BankAccountDTO          _bank;


        public void SetHost(List<AccountAllocation> listHost, BankAccountDTO bankAccount)
        {
            _list = listHost;
            _bank = bankAccount;
            SetItems(_list);
        }


        public void OnAmountChanged(decimal? amount)
        {
            if (!amount.HasValue) return;

            if (!this.Any())
                this.Add(new AccountAllocation
                {
                    Account   = GLAccountDTO.CashInBank(_bank),
                    SubAmount = amount.Value
                });

            var matches = this.Where(_ => _.Account.Id == 0 && _.IsCredit);
            if (matches.Count() == 1)
                matches.Single().SubAmount = amount.Value;
        }
    }
}
