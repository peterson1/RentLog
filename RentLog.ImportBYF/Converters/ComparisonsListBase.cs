using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib11.JsonTools;
using CommonTools.Lib11.StringTools;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using Newtonsoft.Json;
using RentLog.DomainLib11.DataSources;
using RentLog.DomainLib11.MarketStateRepos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RentLog.ImportBYF.Converters
{
    public abstract class ComparisonsListBase : UIList<JsonComparer>
    {
        public ComparisonsListBase(MainWindowVM mainWindowVM)
        {
            MainWindow   = mainWindowVM;
            ImportAllCmd = R2Command.Async(this.ImportAll, _ => CanImportAll(), $"Import {MainWindow.PickedListName}");
        }


        public IR2Command     ImportAllCmd       { get; }
        public MainWindowVM   MainWindow         { get; }
        public int            UnexpectedsCount   { get; private set; }
        public int            ByfCount           { get; private set; }
        public int            RntCount           { get; private set; }
        public int            DiffCount          { get; private set; }

        public abstract List<object>       GetListFromBYF (string cacheDir);
        public abstract List<IDocumentDTO> GetListFromRNT (ITenantDBsDir dir);
        public abstract IDocumentDTO       CastByfToDTO   (object byfRecord);
        public abstract int                GetByfId       (object byfRecord);
        public abstract void               ReplaceAll     (IEnumerable<IDocumentDTO> documents, MarketStateDbBase marketStateDB);

        //public virtual object PreCompareBYF(object obj) => obj;
        //public virtual object PreCompareRNT(object obj) => obj;


        public ITenantDBsDir AppArgs => MainWindow.AppArgs;


        public void ReloadList()
        {
            UIThread.Run(()  => ClearItems());
            UnexpectedsCount = 0;
            var tupl         = this.QueryBothSources();
            var items        = this.AlignByIDs(tupl);
            UIThread.Run(()  => SetItems(items));
            UpdateCounts();
        }


        private void UpdateCounts()
        {
            UnexpectedsCount = ComparisonsAligner.UnexpectedsCount;
            ByfCount  = this.Count(_ => _.Document1 != null);
            RntCount  = this.Count(_ => _.Document2 != null);
            DiffCount = this.Count(_ => _.IsTheSame != true);
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
            if (DiffCount == 0) return false;
            return true;
        }


        protected List<dynamic> ReadJsonList(string displayID)
        {
            var startStr = CacheReader2.appendToDisplayId(displayID);
            var matches  = CacheReader2.findInJsonFiles(MainWindow.ByfCache.CacheDir, startStr);
            var lines    = File.ReadAllText(matches.Single());
            return JsonConvert.DeserializeObject<List<dynamic>>(lines);
        }
    }
}
