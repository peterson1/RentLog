namespace CommonTools.Lib11.JsonTools
{
    public interface IJsonCache
    {
        void Put   <T>(string key, T content);
        T    Get   <T>(string key);
        bool Has      (string key);
        bool TryGet<T>(string key, out T value);
    }
}
