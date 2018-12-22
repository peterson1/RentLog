using PropertyChanged;
using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Converters.BalanceAdjConverters;
using System;
using System.Linq;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalAdjRow
    {
        private IBalanceAdjustmentsRepo _repo;


        public LeaseBalAdjRow(BalanceAdjustmentDTO byfDTO, BalanceAdjConverter1 conv, ITenantDBsDir dir)
        {
            ByfDTO = byfDTO;
            RntDTO = FindRntDTO(conv, dir, out DateTime date);
            Date   = date;
        }


        public DateTime              Date    { get; }
        public BalanceAdjustmentDTO  ByfDTO  { get; }
        public BalanceAdjustmentDTO  RntDTO  { get; }

        public int  Id          => ByfDTO.Id;
        public bool AreEqual    => ByfDTO.Equals(RntDTO);


        public void UpsertDTO()
        {
            if (RntDTO == null)
                _repo.Insert(ByfDTO);
            else
                _repo.Update(ByfDTO);
        } 


        private BalanceAdjustmentDTO FindRntDTO(BalanceAdjConverter1 conv, ITenantDBsDir dir, out DateTime date)
        {
            date       = conv._adjDates[Id];
            var colxns = dir.Collections.For(date);
            _repo      = colxns?.BalanceAdjs;
            return _repo?.Find(Id, false);
            //return _repo?.GetAll().SingleOrDefault(_ => _.Id == Id);
        }
    }
}
