using PropertyChanged;
using RentLog.Cashiering.CommonControls;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.Cashiering.SectionTabs.AmbulantCollections
{
    [AddINotifyPropertyChangedInterface]
    public class AmbulantColxnsVM : EncoderListVMBase<AmbulantColxnDTO>
    {
        private AmbulantColxnCrudVM _crud;


        public AmbulantColxnsVM(SectionDTO sec, MainWindowVM main)
            : base(GetRepo(sec, main), main)
        {
            TotalVisible = false;
            _crud = new AmbulantColxnCrudVM(sec, GetRepo(sec, main), main.AppArgs);
        }


        private static IAmbulantColxnsRepo GetRepo(SectionDTO sec, MainWindowVM main)
            => main.ColxnsDB.AmbulantColxns[sec.Id];

        protected override Func<AmbulantColxnDTO, decimal> SummedAmount => _ => _.Amount;
        protected override string ListTitle => "Ambulant Collections";
        protected override void AddNewItem() => _crud.EncodeNewDraftCmd.ExecuteIfItCan();
        protected override void OnItemOpened(AmbulantColxnDTO e) => _crud.EditCurrentRecord(e);
    }
}
