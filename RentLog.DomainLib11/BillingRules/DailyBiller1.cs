using RentLog.DomainLib11.CollectionRepos;
using RentLog.DomainLib11.DTOs;
using System;
using static RentLog.DomainLib11.DTOs.DailyBillDTO;

namespace RentLog.DomainLib11.BillingRules
{
    public class DailyBiller1 : IDailyBiller
    {
        private ICollectionsDB _colxnsDB;

        public DailyBiller1(ICollectionsDB collectionsDB)
        {
            _colxnsDB = collectionsDB;
        }


        public BillState ComputeBill(DailyBillDTO bill, BillCode billCode)
        {
            throw new NotImplementedException();
        }
    }
}
