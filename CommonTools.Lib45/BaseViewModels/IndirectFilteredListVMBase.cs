using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.ThreadTools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib45.BaseViewModels
{
    public abstract class IndirectFilteredListVMBase<TRow, TDTO, TFilter, TArg>
        : SavedListVMBase<TDTO, TArg>, IEnumerable<TRow>
            where TRow    : IHasDTO<TDTO>
            where TDTO    : class, IDocumentDTO
            where TFilter : TextFilterBase<TRow>, new()
            where TArg    : ICredentialsProvider
    {
        private List<TRow> _cache;


        public IndirectFilteredListVMBase(ISimpleRepo<TDTO> simpleRepo, TArg appArguments, bool doReload = true) : base(simpleRepo, appArguments, doReload)
        {
            Filter.TextFilterChanged += (s, e) => ApplyTextFilters();
            Rows.ItemDeleted         += (s, e) => ExecuteDeleteRecord(e.DTO);
            Rows.ItemOpened          += (s, e) =>
           {
               OnItemOpened(e.DTO);
               UpdateTotalSum();
           };
        }


        public UIList<TRow>  Rows    { get; } = new UIList<TRow>();
        public TFilter       Filter  { get; } = new TFilter();


        protected abstract TRow CastToRow(TDTO dto);


        public override void ReloadFromDB()
        {
            var dtos = GetPostProcessedResult().ToList();
            _cache   = dtos.Select(_ => CastToRow(_)).ToList();
            ApplyTextFilters();
        }


        private void ApplyTextFilters()
        {
            var list = _cache.ToList();
            Filter.RemoveNonMatches(ref list);
            UIThread.Run(() => Rows.SetItems(list));
        }


        protected override bool TryGetPickedItem(out TDTO dto)
        {
            dto = Rows.CurrentItem?.DTO;
            return dto != null;
        }


        public new IEnumerator<TRow> GetEnumerator() => ((IEnumerable<TRow>)Rows).GetEnumerator();
             IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TRow>)Rows).GetEnumerator();
    }
}
