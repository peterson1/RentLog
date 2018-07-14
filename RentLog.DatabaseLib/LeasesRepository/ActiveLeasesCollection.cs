using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.LeasesRepository
{
    internal class ActiveLeasesCollection : SharedCollectionBase<LeaseDTO>, ISimpleRepo<LeaseDTO>
    {
        private const string COLXN_NAME = "ActiveLeases";


        internal ActiveLeasesCollection(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
            BsonMapper.Global.Entity<LeaseDTO>()
                .DbRef(_ => _.Stall, StallsCollection.COLXN_NAME);
        }


        public override LiteCollection<LeaseDTO> GetCollection(LiteDatabase db)
            => db.GetCollection<LeaseDTO>(COLXN_NAME)
                    .Include(_ => _.Stall)
                    .Include(_ => _.Stall.Section);


        public override bool TableExists()
            => _db.OpenRead().CollectionExists(COLXN_NAME);
    }
}
