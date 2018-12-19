using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.DTOs;
using RentLog.ImportBYF.Converters.BalanceAdjConverters;
using System;

namespace RentLog.ImportBYF.Remediations.VerifyLeaseMemos
{
    [AddINotifyPropertyChangedInterface]
    public class LeaseBalAdjRow
    {
        public LeaseBalAdjRow(BalanceAdjustmentDTO byfDTO, BalanceAdjConverter1 conv, ITenantDBsDir dir)
        {
            ByfDTO = byfDTO;
            RntDTO = FindRntDTO(conv, dir, out DateTime date);
            Date   = date;
        }


        public DateTime              Date    { get; }
        public BalanceAdjustmentDTO  ByfDTO  { get; }
        public BalanceAdjustmentDTO  RntDTO  { get; }

        public int Id => ByfDTO.Id;

        //todo: create AreEqual auto-property


        private BalanceAdjustmentDTO FindRntDTO(BalanceAdjConverter1 conv, ITenantDBsDir dir, out DateTime date)
        {
            date     = conv._adjDates[Id];
            var repo = dir.Collections.For(date).BalanceAdjs;
            return repo.Find(Id, false);
        }
    }
}
