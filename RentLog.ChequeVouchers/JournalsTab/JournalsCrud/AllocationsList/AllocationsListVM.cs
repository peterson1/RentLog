using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.VoucherRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsCrud.AllocationsList
{
    [AddINotifyPropertyChangedInterface]
    public class AllocationsListVM : UIList<AccountAllocation>
    {
        private List<AccountAllocation> _list;
        private List<GLAccountDTO>      _glAccts;


        public AllocationsListVM()
        {
            AddDebitCmd  = R2Command.Relay(_ => AddEntry(-1), null, "Add Debit entry");
            AddCreditCmd = R2Command.Relay(_ => AddEntry(+1), null, "Add Credit entry");
            ItemOpened  += AllocationsListVM_ItemOpened;
            ItemDeleted += AllocationsListVM_ItemDeleted;
        }


        public void SetHost(List<AccountAllocation> listHost, IGLAccountsRepo glAcctsRepo)
        {
            _list    = listHost;
            _glAccts = glAcctsRepo.AllWithCashInBanks();
            UpdateUILists();
        }


        public IR2Command  AddDebitCmd   { get; }
        public IR2Command  AddCreditCmd  { get; }


        private void AddEntry(decimal multiplier)
        {
            var typ = multiplier < 0 ? "Debit" : "Credit";

            if (!PopUpInput.TryGetIndex($"GL Account for {typ}", 
                out int idx, _glAccts)) return;

            if (!PopUpInput.TryGetDecimal($"{typ} Amount",
                out decimal amt, allowZero: false)) return;

            _list.Add(AccountAllocation
                .NewItem(_glAccts[idx], amt, multiplier));

            UpdateUILists();
        }


        private void AllocationsListVM_ItemOpened(object sender, AccountAllocation e)
        {
            var oldIndx = _glAccts.FindIndex(_ => _.Id == e.Account.Id);

            if (!PopUpInput.TryGetIndex("GL Account",
                out int newIndx, _glAccts, oldIndx)) return;

            if (!PopUpInput.TryGetDecimal("Amount",
                out decimal amt, Math.Abs(e.SubAmount), allowZero: false)) return;

            e.Account   = _glAccts[newIndx];
            e.SubAmount = amt * (e.IsDebit ? -1M : 1M);

            UpdateUILists();
        }


        private void AllocationsListVM_ItemDeleted(object sender, AccountAllocation e)
        {
            _list.Remove(e);
            UpdateUILists();
        }


        private void UpdateUILists()
        {
            SetItems(_list);
            SetSummary(new AllocationsTotal(this));
        }


        public class AllocationsTotal : AccountAllocation
        {
            public AllocationsTotal(IEnumerable<AccountAllocation> items)
            {
                AsDebit  = items.Sum(_ => _.AsDebit  ?? 0);
                AsCredit = items.Sum(_ => _.AsCredit ?? 0);
            }

            public override decimal? AsDebit  { get; }
            public override decimal? AsCredit { get; }
        }
    }
}
