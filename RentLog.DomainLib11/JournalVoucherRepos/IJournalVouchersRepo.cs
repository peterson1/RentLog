using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;

namespace RentLog.DomainLib11.JournalVoucherRepos
{
    public interface IJournalVouchersRepo : ISimplerRepo<JournalVoucherDTO>
    {
        List<JournalVoucherDTO> List(DateTime startDate, DateTime endDate);
    }
}
