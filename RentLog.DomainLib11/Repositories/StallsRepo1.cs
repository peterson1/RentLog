using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.DomainLib11.Repositories
{
    public class StallsRepo1 : IStallsRepo
    {
        public event EventHandler<StallDTO> ContentChanged = delegate { };

        private ISimpleRepo<StallDTO> _simpl;
        private AllRepositories _db;


        public StallsRepo1(ISimpleRepo<StallDTO> simpleRepo, AllRepositories allRepositories)
        {
            _db    = allRepositories;
            _simpl = simpleRepo;
            _simpl.ContentChanged += (s, e) => ContentChanged?.Invoke(s, e);
        }


        public List<StallDTO>  GetAll ()                       => _simpl.GetAll ();
        public bool            Update (StallDTO changedRecord) => _simpl.Update (changedRecord);

        public int Insert(StallDTO newRecord)
        {
            this.RejectDuplicateName(newRecord);
            return _simpl.Insert(newRecord);
        }


        public bool Delete(StallDTO record)
        {
            this.DontDeleteIfOccupied(record, _db.ActiveLeases);
            return _simpl.Delete(record);
        }




        public List<StallDTO> ForSection(SectionDTO section)
            => GetAll().Where(_ => _.Section.Id == section.Id).ToSortedList();
    }


    internal static class StallsRepo1Ext
    {
        internal static List<StallDTO> ToSortedList(this IEnumerable<StallDTO> items)
            => items.OrderBy(_ => _.Name).ToList();
    }
}
