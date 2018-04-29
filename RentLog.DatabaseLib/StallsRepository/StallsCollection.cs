using CommonTools.Lib45.LiteDbTools;
using LiteDB;
using RentLog.DomainLib11.DTOs;

namespace RentLog.DatabaseLib.StallsRepository
{
    public class StallsCollection : NamedCollectionBase<StallDTO>
    {
        private const string COLXN_NAME = "StallModel";


        public StallsCollection(string dbFilePath, string currentUser) 
            : base(COLXN_NAME, new RentLogDB(dbFilePath, currentUser))
        {
            BsonMapper.Global.Entity<StallDTO>()
                .DbRef(_ => _.Section, "SectionModel");
        }


        public override LiteCollection<StallDTO> GetCollection(LiteDatabase db)
            => db.GetCollection<StallDTO>(COLXN_NAME)
                 .Include(_ => _.Section);
    }
}
