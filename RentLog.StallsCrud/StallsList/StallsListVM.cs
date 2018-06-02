using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.BaseViewModels;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.InputDialogs;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using RentLog.DomainLib11.Authorization;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45;
using RentLog.StallsCrud.StallCRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentLog.StallsCrud.StallsList
{
    [AddINotifyPropertyChangedInterface]
    public class StallsListVM : IndirectFilteredListVMBase<StallRow, StallDTO, StallsFilterVM, AppArguments>
    {
        private Dictionary<int, LeaseDTO> _stallIdToLeases;
        private LeaseDTO _vacant = GetVacantPlaceHoder();
        private MainWindowVM _main;


        public StallsListVM(MainWindowVM mainWindowVM, AppArguments appArguments) : base(appArguments.MarketState.Stalls, appArguments, false)
        {
            _main            = mainWindowVM;
            Crud             = new StallCrudVM(appArguments);
            _stallIdToLeases = AppArgs.MarketState.ActiveLeases.StallsLookup();
            AddMultipleCmd   = R2Command.Async(AddMultipleStalls, _ => !_main.IsBusy, "Add Multiple Stalls");
        }


        public StallCrudVM  Crud            { get; }
        public IR2Command   AddMultipleCmd  { get; }


        private async Task AddMultipleStalls(object cmdParam)
        {
            if (!PopUpInput.TryGetInt("How many stalls are we adding?", out int count, 10, "Please enter the number of stalls to add")) return;
            _main.StartBeingBusy($"Adding {count} stalls ...");

            for (int i = 0; i < count; i++)
            {
                Crud.EncodeNewDraftCmd.ExecuteIfItCan();
                await Task.Delay(100);
                await Crud.SaveDraftCmd.RunAsync();
            }

            _main.StopBeingBusy();
        }


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
