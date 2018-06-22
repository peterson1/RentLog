using CommonTools.Lib11.DatabaseTools;
using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using System;

namespace RentLog.Cashiering.OtherCollections
{
    [AddINotifyPropertyChangedInterface]
    public class OtherColxnsVM : EncoderListVMBase<OtherColxnDTO>
    {
        private OtherColxnCrudVM _crud;


        public OtherColxnsVM(MainWindowVM main) : base(main.ColxnsDB.OtherColxns, main)
        {
            _crud = new OtherColxnCrudVM(main.ColxnsDB?.OtherColxns, main.AppArgs);
        }


        protected override Func<OtherColxnDTO, decimal> SummedAmount => _ => _.Amount;
        protected override string ListTitle => "Other Collections";
        protected override void AddNewItem() => _crud.EncodeNewDraftCmd.ExecuteIfItCan();
        protected override void LoadRecordForEditing(OtherColxnDTO rec) => _crud.EditCurrentRecord(rec);
    }
}
