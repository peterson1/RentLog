using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.StallsCrud.StallCRUD;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.StallsCrud.StallsList
{
    [AddINotifyPropertyChangedInterface]
    public class StallsListVM : IndirectFilteredListVMBase<StallRow, StallDTO, StallsFilterVM, AppArguments>
    {
        private Dictionary<int, LeaseDTO> _stallIdToLeases;
        private LeaseDTO _vacant = GetVacantPlaceHoder();


        public StallsListVM(AppArguments appArguments) : base(appArguments.MarketState.Stalls, appArguments, false)
        {
            Crud             = new StallCrudVM(appArguments);
            _stallIdToLeases = AppArgs.MarketState.ActiveLeases.StallsLookup();
        }


        public StallCrudVM  Crud     { get; }


        protected override List<StallDTO> QueryItems(ISimpleRepo<StallDTO> db)
            => AppArgs.MarketState.Stalls.ForSection(AppArgs.CurrentSection);


        protected override StallRow CastToRow(StallDTO dto)
        {
            var row      = new StallRow(dto);
            row.Occupant = _stallIdToLeases.TryGetValue(dto.Id, out LeaseDTO lse)
                         ? lse : _vacant;
            return row;
        }


        public override bool CanEditRecord(StallDTO rec)
            => AppArgs.CanEditStall(true);


        protected override bool CanDeletetRecord(StallDTO rec)
        {
            if (!AppArgs.CanDeleteStall(true)) return false;
            if (_stallIdToLeases.TryGetValue(rec.Id, out LeaseDTO lse))
            {
                Alert.Show("Cannot Delete if Occupied", $"Currently occupied by {lse}");
                return false;
            }
            return true;
        }


        protected override void LoadRecordForEditing(StallDTO rec)
        {
            Crud.Occupant = Rows.SingleOrDefault(_ => _.DTO.Id == rec.Id)?.Occupant;
            Crud.EditCurrentRecord(rec);
        }

        private static LeaseDTO GetVacantPlaceHoder()
            => new LeaseDTO
            {
                Tenant = new TenantModel { LastName = "--" }
            };
    }
}
