using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.BaseViewModels;
using RentLog.DomainLib11.Authorization;

namespace RentLog.Cashiering.CommonControls
{
    public abstract class EncoderListVMBase<TDTO, TArg> : SavedListVMBase<TDTO, TArg>
        where TDTO : IDocumentDTO
        where TArg : ICredentialsProvider
    {
        public EncoderListVMBase(ISimpleRepo<TDTO> repository, TArg appArguments) : base(repository, appArguments, false)
        {
            CanEncode = AppArgs.CanEncodeCollections(false);
            Caption   = ListTitle;
        }


        public bool  CanEncode  { get; }


        protected abstract string ListTitle { get; }
    }
}
