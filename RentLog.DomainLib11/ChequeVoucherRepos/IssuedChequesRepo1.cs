﻿//using CommonTools.Lib11.DatabaseTools;
//using RentLog.DomainLib11.DTOs;
//using System.Collections.Generic;
//using System.Linq;

//namespace RentLog.DomainLib11.ChequeVoucherRepos
//{
//    public class IssuedChequesRepo1 : SimpleRepoShimBase<ChequeVoucherDTO>, IChequeVouchersRepo
//    {
//        public IssuedChequesRepo1(ISimpleRepo<ChequeVoucherDTO> simpleRepo) : base(simpleRepo)
//        {
//        }


//        public override List<ChequeVoucherDTO> GetAll()
//            => base.GetAll().Where(_ => _.IssuedDate.HasValue).ToList();
//    }
//}