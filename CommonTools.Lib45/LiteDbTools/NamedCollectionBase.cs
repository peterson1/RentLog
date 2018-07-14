using CommonTools.Lib11.DTOs;
using LiteDB;

namespace CommonTools.Lib45.LiteDbTools
{
    public abstract class NamedCollectionBase<T> : SharedCollectionBase<T>
        where T : IDocumentDTO
    {
        public NamedCollectionBase(string collectionName, SharedLiteDB sharedLiteDB) : base(sharedLiteDB)
        {
            CollectionName = collectionName;
        }


        public string CollectionName { get; }


        public override LiteCollection<T> GetCollection(LiteDatabase db)
            => db.GetCollection<T>(CollectionName);


        public override bool TableExists()
            => _db.OpenRead().CollectionExists(CollectionName);
    }
}
