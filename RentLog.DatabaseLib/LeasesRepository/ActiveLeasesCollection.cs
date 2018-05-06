﻿using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.LeasesRepository
{
    public class ActiveLeasesCollection : NamedCollectionBase<LeaseDTO>
    {
        private const string COLXN_NAME = "ActiveLeases";


        public ActiveLeasesCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
            BsonMapper.Global.Entity<LeaseDTO>()
                .DbRef(_ => _.Stall, StallsCollection.COLXN_NAME);
        }


        public override LiteCollection<LeaseDTO> GetCollection(LiteDatabase db)
            => db.GetCollection<LeaseDTO>(COLXN_NAME)
                    .Include(_ => _.Stall);
    }
}