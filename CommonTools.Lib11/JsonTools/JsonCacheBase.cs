namespace CommonTools.Lib11.JsonTools
{
    public abstract class JsonCacheBase : IJsonCache
    {
        protected abstract string  GetFilePath    (string key);
        protected abstract bool    FileExists     (string filePath);
        protected abstract string  ReadTextFile   (string filePath);
        protected abstract void    WriteTextFile  (string filePath, string content);


        public void  Put  <T>(string key, T content)
        {
            var json = content.ToJson(false);
            var path = GetFilePath(key);
            WriteTextFile(path, json);
        }


        public T Get<T>(string key)
        {
            var json = ReadTextFile(GetFilePath(key));
            return json.ReadJson<T>();
        }


        public bool TryGet<T>(string key, out T value)
        {
            if (Has(key))
            {
                value = Get<T>(key);
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }


        public bool Has(string key) => FileExists(GetFilePath(key));
    }
}
