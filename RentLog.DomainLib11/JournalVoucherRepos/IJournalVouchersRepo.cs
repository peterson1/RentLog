using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentLog.DomainLib11.JournalVoucherRepos
{
    public interface IJournalVouchersRepo : ISimplerRepo<JournalVoucherDTO>
    {
        Task<int>                      GetNextSerialNum ();
        List<JournalVoucherDTO>        List             (DateTime startDate, DateTime endDate);
        ISimpleRepo<JournalVoucherDTO> GetSoloShard     (DateTime date);
    }
}
