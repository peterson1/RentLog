namespace CommonTools.Lib11.DatabaseTools
{
    public interface ISimplerRepo<T>
    {
        int   Insert            (T newRecord);
        bool  Update            (T changedRecord);
        bool  Delete            (T unwantedRecord);

        bool  IsValidForInsert  (T draft , out string whyInvalid);
        bool  IsValidForUpdate  (T record, out string whyInvalid);
        bool  IsValidForDelete  (T record, out string whyInvalid);
    }
}
