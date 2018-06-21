namespace CommonTools.Lib11.DatabaseTools
{
    public interface IAddEditRepo<T>
    {
        int   Insert            (T newRecord);
        bool  Update            (T changedRecord);

        bool  IsValidForInsert  (T draft, out string whyInvalid);
        bool  IsValidForUpdate  (T record, out string whyInvalid);
    }
}
