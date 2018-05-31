﻿using CommonTools.Lib11.ExceptionTools;
using RentLog.DomainLib11.BalanceRepos;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib11.MarketStateRepos;
using System;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class LeaseDeactivator
    {
        public static InactiveLeaseDTO DeactivateLease(this MarketStateDB mkt, LeaseDTO lse,
            string reason, DateTime deactivationDate)
        {
            if (lse is InactiveLeaseDTO)
                throw Bad.State<LeaseDTO>("Inactive", "Active");

            var inactv = new InactiveLeaseDTO(lse, reason, deactivationDate, mkt.CurrentUser);
            mkt.InactiveLeases.Insert(inactv);

            return inactv;
        }
    }
}