namespace CommonTools.Lib11.DatabaseTools
{
    public interface IKeyValueStore
    {
        string  this       [string key] { get; set; }
        bool    Has        (string key);
        bool    IsTrue     (string key);
        void    SetTrue    (string key);
    }
}
