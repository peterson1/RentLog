using RentLog.DomainLib11.DataSources;
using RentLog.ImportBYF.Version2UI.JournalVouchersPane.JVsByDateList;
using System;

namespace RentLog.ImportBYF.RntQueries
{
    public static class RntJVsByDateQueries
    {
        public static JVsByDateCell GetJournalVouchers(this ITenantDBsDir dir, DateTime date)
            => new JVsByDateCell { DTOs = dir.Journals.List(date, date) };
    }
}
