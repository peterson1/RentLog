using CommonTools.Lib11.DatabaseTools;
using RentLog.DomainLib11.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentLog.DomainLib11.PassbookRepos
{
    public class PassbookRowsSimpleRepo : SimpleRepoShimBase<PassbookRowDTO>
    {
        private int _bankAcctID;


        public PassbookRowsSimpleRepo(int bankAccountID, ISimpleRepo<PassbookRowDTO> simpleRepo) : base(simpleRepo)
        {
            _bankAcctID = bankAccountID;
        }


        public override bool IsValidForInsert(PassbookRowDTO draft, out string whyInvalid)
        {
            //todo: test validations
            return base.IsValidForInsert(draft, out whyInvalid);
        }

    }
}
