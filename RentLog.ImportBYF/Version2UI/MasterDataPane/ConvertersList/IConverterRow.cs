using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.JsonTools;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    public interface IConverterRow
    {
        string  Label        { get; }
        int     ByfCount     { get; }
        int     RntCount     { get; }
        int     DiffsCount   { get; }
        int     Unexpecteds  { get; }
        bool    IsBusy       { get; }
        string  BusyText     { get; }
        string  ErrorText    { get; }

        IR2Command  RefreshCmd    { get; }
        IR2Command  UpdateRntCmd  { get; }

        UIList<JsonComparer> DiffRows { get; }
    }
}
