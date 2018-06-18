using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;

namespace CommonTools.Lib45.LiteDbTools
{
    public class MetadataCollection : NamedCollectionBase<KeyValuePairDTO>, IKeyValueStore
    {
        private const string COLXN_NAME = "DbMetadata";

        public MetadataCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }


        public string this[string key]
        {
            get => ByName(key, false)?.Value;
            set { UpsertByName(key, value); }
        }


        internal void CreateInitialRecord() 
            => Insert(new KeyValuePairDTO("version", "0.01"));


        public bool IsTrue(string key)
        {
            if (!TryGetName(key, 
                out KeyValuePairDTO dto)) return false;

            switch (dto.Value.Trim().ToLower())
            {
                case "true": return true;
                case "yes" : return true;
                case "oo"  : return true;
                case "1"   : return true;
                default: return false;
            }
        }


        private void UpsertByName(string key, string value)
        {
            if (TryGetName(key, out KeyValuePairDTO rec))
            {
                rec.Value = value;
                Update(rec);
            }
            else
                InsertNewPair(key, value);
        }


        public bool Has     (string key) => HasName(key);
        public void SetTrue (string key) => this[key] = "true";


        private void InsertNewPair(string key, string value) 
                => Insert(new KeyValuePairDTO(key, value));

    }
}
