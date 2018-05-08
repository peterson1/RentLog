using CommonTools.Lib11.DTOs;
using System;

namespace CommonTools.Lib45.LiteDbTools
{
    public class MetadataCollection : NamedCollectionBase<KeyValuePairDTO>
    {
        private const string COLXN_NAME = "DbMetadata";

        public MetadataCollection(SharedLiteDB sharedLiteDB) : base(COLXN_NAME, sharedLiteDB)
        {
        }


        public string this[string key]
        {
            get => ByName(key, false)?.Value;
            set
            {
                if (TryGetName(key, out KeyValuePairDTO rec))
                {
                    rec.Value = value;
                    Update(rec);
                }
                else
                    AddPair(key, value);
            }
        }


        private void AddPair(string key, string value) 
                => Insert(new KeyValuePairDTO(key, value));


        internal void CreateInitialRecord() 
            => Insert(new KeyValuePairDTO("version", "0.01"));


        public void AddIfNone(string key, string value)
        {
            if (!HasName(key))
                AddPair(key, value);
        }
    }
}
