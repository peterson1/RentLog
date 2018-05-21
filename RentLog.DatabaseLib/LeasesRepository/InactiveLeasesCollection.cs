using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DatabaseLib.StallsRepository;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.LeasesRepository
{
    internal class InactiveLeasesCollection : SharedCollectionBase<InactiveLeaseDTO>, ISimpleRepo<InactiveLeaseDTO>
    {
        private const string COLXN_NAME = "InactiveLeases";


        internal InactiveLeasesCollection(SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
            BsonMapper.Global.Entity<LeaseDTO>()
                .DbRef(_ => _.Stall, StallsCollection.COLXN_NAME);
        }


        public override LiteCollection<InactiveLeaseDTO> GetCollection(LiteDatabase db)
            => db.GetCollection<InactiveLeaseDTO>(COLXN_NAME)
                    .Include(_ => _.Stall)
                    .Include(_ => _.Stall.Section);
    }
}
