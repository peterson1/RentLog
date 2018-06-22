using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.CashierCollections
{
    [AddINotifyPropertyChangedInterface]
    public class CashierColxnsVM : EncoderListVMBase<CashierColxnDTO>
    {
        private CashierColxnCrudVM _crud;


        public CashierColxnsVM(MainWindowVM main) : base(main.ColxnsDB.CashierColxns, main)
        {
            _crud = new CashierColxnCrudVM(main.ColxnsDB?.CashierColxns, main.AppArgs);
        }


        protected override string ListTitle => "Tenant Payments to Office";
        protected override Func<CashierColxnDTO, LeaseDTO> LeaseGetter => _ => _.Lease;
        protected override Func<CashierColxnDTO, decimal>  SummedAmount => _ => _.Amount;
        protected override void AddNewItem() => _crud.EncodeNewDraftCmd.ExecuteIfItCan();
        protected override void LoadRecordForEditing(CashierColxnDTO rec) => _crud.EditCurrentRecord(rec);

    }
}
