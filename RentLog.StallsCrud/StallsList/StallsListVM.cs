using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using PropertyChanged;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Models;
using RentLog.DomainLib45;
using RentLog.DomainLib45.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.StallsCrud.StallsList
{
    [AddINotifyPropertyChangedInterface]
    class StallsListVM : IndirectFilteredListVMBase<StallRowVM, StallDTO, StallsFilterVM, AppArguments>
    {
        private Dictionary<int, LeaseDTO> _stallIdToLeases;
        private LeaseDTO _vacant = GetVacantPlaceHoder();


        public StallsListVM(AppArguments appArguments) : base(appArguments.Stalls, appArguments, false)
        {
            _stallIdToLeases = AppArgs.ActiveLeases.GetAll().ToDictionary(_ => _.Stall.Id);
        }


        public SectionDTO Section { get; internal set; }


        protected override List<StallDTO> QueryItems(ISimpleRepo<StallDTO> db)
            => AppArgs.Stalls.ForSection(Section);


        protected override StallRowVM CastToRow(StallDTO dto)
        {
            var row      = new StallRowVM(dto);
            //if (_stallIdToLeases.TryGetValue(dto.Id, out LeaseDTO lse))
            //    row.Occupant = lse;
            row.Occupant = _stallIdToLeases.TryGetValue(dto.Id, out LeaseDTO lse)
                         ? lse : _vacant;
            return row;
        }

        protected override Func<StallDTO, decimal> SummedAmount => throw new NotImplementedException();

        private static LeaseDTO GetVacantPlaceHoder()
            => new LeaseDTO
            {
                Tenant = new TenantModel { LastName = "--" }
            };
    }
}
