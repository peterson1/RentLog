using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DatabaseLib.SectionsRepository;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.StallsRepository
{
    public class StallsCollection : NamedCollectionBase<StallDTO>
    {
        internal const string COLXN_NAME = "StallModel";


        public StallsCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
            BsonMapper.Global.Entity<StallDTO>()
                .DbRef(_ => _.Section, SectionsCollection.COLXN_NAME);
        }


        public override LiteCollection<StallDTO> GetCollection(LiteDatabase db)
            => db.GetCollection<StallDTO>(COLXN_NAME)
                 .Include(_ => _.Section);
    }
}
