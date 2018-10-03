using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;

namespace RentLog.ImportBYF.Converters
{
    public abstract class ComparisonsListBase : UIList<JsonComparer>
    {
        public ComparisonsListBase(MainWindowVM mainWindowVM)
        {
            MainWindow   = mainWindowVM;
            ImportAllCmd = R2Command.Async(this.ImportAll, _ => CanImportAll(), "Import All");
        }


        public IR2Command     ImportAllCmd       { get; }
        public MainWindowVM   MainWindow         { get;}
        public int            UnexpectedsCount   { get; private set; }

        public abstract List<object>       GetListFromBYF (string cacheDir);
        public abstract List<IDocumentDTO> GetListFromRNT (ITenantDBsDir dir);
        public abstract IDocumentDTO       CastByfToDTO   (object byfRecord);
        public abstract int                GetByfId       (object byfRecord);
        public abstract void               ReplaceAll     (IEnumerable<IDocumentDTO> documents, MarketStateDB marketStateDB);

        public ITenantDBsDir AppArgs => MainWindow.AppArgs;


        public void ReloadList()
        {
            UIThread.Run(()  => ClearItems());
            UnexpectedsCount = 0;
            var tupl         = this.QueryBothSources();
            var items        = tupl.AlignByIDs();
            UnexpectedsCount = ComparisonsAligner.UnexpectedsCount;
            UIThread.Run(()  => SetItems(items));
        }


        protected T Cast<T>(object rec)
        {
            if (rec is T casted)
                return casted;
            else
            {
                var typ = typeof(T).Name;
                var msg = $"Failed to cast as ‹{typ}›"
                        + L.F + rec.ToJson();
                throw new InvalidCastException(msg);
            }
        }


        private bool CanImportAll()
        {
            if (UnexpectedsCount != 0) return false;
            return true;
        }
    }
}
