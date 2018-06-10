using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.StateTransitions
{
    public static class ChequeClearedToPassbook
    {
        public static PassbookRowDTO ToPassbookRow(this ChequeVoucherDTO cheque)
        {
            throw new NotImplementedException();
        }
    }
}
