using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputDialogs;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib45.SoaViewers.AdjustmentsCRUD
{
    public class AdjustmentsListVM : SavedListVMBase<BalanceAdjustmentDTO, AppArguments>
    {
        private int                     _lseId;
        private IBalanceAdjustmentsRepo _repo;


        public AdjustmentsListVM(int leaseId, BillCode billCode, DateTime date, AppArguments args) : base(GetRepo(date, args), args, false)
        {
            _lseId   = leaseId;
            _repo    = GetRepo(date, AppArgs);
            BillCode = billCode;
            ReloadFromDB();
        }


        public BillCode BillCode { get; }


        protected override void AddNewItem()
        {
            (string Reason, decimal Amount, string DocumentRef) tupl;
            if (!TryGetInput(out tupl)) return;
            Save(new BalanceAdjustmentDTO
            {
                LeaseId      = _lseId,
                BillCode     = BillCode,
                DocumentRef  = tupl.DocumentRef,
                AmountOffset = tupl.Amount,
                Reason       = tupl.Reason,
            });
        }


        protected override void OnItemOpened(BalanceAdjustmentDTO e)
        {
            (string Reason, decimal Amount, string DocumentRef) tupl;
            if (!TryGetInput(out tupl, e.Reason, e.AmountOffset, e.DocumentRef)) return;
            e.Reason       = tupl.Reason;
            e.AmountOffset = tupl.Amount;
            e.DocumentRef  = tupl.DocumentRef;
            Save(e);
        }


        private bool TryGetInput(out (string Reason, decimal Amount, string DocumentRef) tupl, string oldReason = null, decimal? oldAmount = null, string oldDocRef = null)
        {
            tupl = (null, 0, null);
            if (!PopUpInput.TryGetString("Reason for adjustment", out string reason, oldReason)) return false;
            if (!PopUpInput.TryGetDecimal("Adjustment Amount", out decimal amount, oldAmount)) return false;
            PopUpInput.TryGetString("PR #", out string docRef, oldDocRef);
            tupl.Reason      = reason;
            tupl.Amount      = amount;
            tupl.DocumentRef = docRef;
            return true;
        }


        private void Save(BalanceAdjustmentDTO balanceAdjustmentDTO)
        {
            _repo.Upsert(balanceAdjustmentDTO);
            ReloadFromDB();
        }


        protected override List<BalanceAdjustmentDTO> QueryItems(ISimpleRepo<BalanceAdjustmentDTO> db)
            => db.GetAll().Where(_ => _.LeaseId  == _lseId
                                   && _.BillCode == BillCode).ToList();


        private static IBalanceAdjustmentsRepo GetRepo(DateTime date, AppArguments args)
            => args.Collections.For(date).BalanceAdjs;


        protected override Func<BalanceAdjustmentDTO, decimal> SummedAmount => _ => _.AmountOffset;
    }
}
