using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;

namespace RentLog.DomainLib11.CollectionRepos
{
    public interface IBalanceAdjustmentsRepo : ISimpleRepo<BalanceAdjustmentDTO>
    {
        DateTime Date { get; }
    }
}
