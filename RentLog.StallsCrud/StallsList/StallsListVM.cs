using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45;
using RentLog.DomainLib45.Repositories;
using System;
using System.Collections.Generic;

namespace RentLog.StallsCrud.StallsList
{
    class StallsListVM : IndirectFilteredListVMBase<StallRowVM, StallDTO, StallsFilterVM, AppArguments>
    {
        private StallsRepo _repo;

        public StallsListVM(StallsRepo stallsRepo, AppArguments appArguments, bool doReload = true) : base(stallsRepo, appArguments, doReload)
        {
            _repo = stallsRepo;
        }


        public SectionDTO Section { get; internal set; }


        protected override List<StallDTO> QueryItems(ISimpleRepo<StallDTO> db)
            => _repo.ForSection(Section);


        protected override StallRowVM CastToRow(StallDTO dto)
            => new StallRowVM(dto);


        protected override Func<StallDTO, decimal> SummedAmount => throw new NotImplementedException();
    }
}
