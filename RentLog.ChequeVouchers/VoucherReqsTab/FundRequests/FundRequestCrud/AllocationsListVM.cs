using CommonTools.Lib11.DataStructures;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using static RentLog.DomainLib11.DTOs.FundRequestDTO;

namespace RentLog.ChequeVouchers.VoucherReqsTab.FundRequests.FundRequestCrud
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
            //if (!amount.HasValue) return;
            this.Add(new AccountAllocation
            {
                Account   = GLAccountDTO.CashInBank(_bank),
                SubAmount = amount.Value
            });
        }
    }
}
