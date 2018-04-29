using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.LiteDbTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.Lib45.BaseViewModels
{
    public abstract class IndirectFilteredListVMBase<TRow, TDTO, TFilter, TArg>
        : FilteredSavedListVMBase<TDTO, TFilter, TArg>
            where TRow : IHasDTO<TDTO>
            where TDTO : IDocumentDTO
            where TFilter : TextFilterBase<TDTO>, new()
            where TArg : ICredentialsProvider
    {
        public IndirectFilteredListVMBase(SharedCollectionBase<TDTO> sharedCollection, TArg appArguments, bool doReload = true) : base(sharedCollection, appArguments, doReload)
        {
        }
    }
}
