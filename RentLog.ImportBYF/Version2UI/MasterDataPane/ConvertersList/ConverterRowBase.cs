using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.MarketStateRepos;
using System.Collections.Generic;
using System.Linq;

namespace RentLog.ImportBYF.Version2UI.MasterDataPane.ConvertersList
{
    [AddINotifyPropertyChangedInterface]
    public abstract partial class ConverterRowBase<T> : IConverterRow
        where T : class, IDocumentDTO
    {

        public ConverterRowBase(MainWindowVM2 mainWindowVM2)
        {
            Main         = mainWindowVM2;
            RefreshCmd   = R2Command.Async(this.GetDifferences, _ => !IsBusy, "Get Diffs");
            UpdateRntCmd = R2Command.Async(this.UpdateRnt, _ => this.CanUpdateRnt(), "Update RNT");
        }

        public abstract string   Label           { get; }
        public abstract string   ViewsDisplayID  { get; }
        public abstract T        CastToRNT       (dynamic byf);
        public abstract List<T>  GetRntRecords   (ITenantDBsDir dir);
        public abstract void     ReplaceAll      (IEnumerable<T> newRecords, MarketStateDB mkt);


        public int     ByfCount     { get; private set; }
        public int     RntCount     { get; private set; }
        public int     DiffsCount   { get; private set; }
        public int     Unexpecteds  { get; set; }
        public bool    IsBusy       { get; private set; }
        public string  BusyText     { get; private set; }
        public string  ErrorText    { get; set; }

        public IR2Command  RefreshCmd    { get; }
        public IR2Command  UpdateRntCmd  { get; }

        public MainWindowVM2         Main      { get; }
        public UIList<JsonComparer>  DiffRows  { get; } = new UIList<JsonComparer>();


        public void UpdateCounts()
        {
            ByfCount    = DiffRows.Count(_ => _.Document1 != null);
            RntCount    = DiffRows.Count(_ => _.Document2 != null);
            DiffsCount  = DiffRows.Count(_ => _.IsTheSame != true);
        }


        public void StartBeingBusy (string message)
        {
            IsBusy   = true;
            BusyText = message;
        }
        public void StopBeingBusy  (string message = null)
        {
            IsBusy   = false;
            BusyText = message;
        }
        public void LogError       (string errorText) => ErrorText += L.F + errorText;
    }
}
 