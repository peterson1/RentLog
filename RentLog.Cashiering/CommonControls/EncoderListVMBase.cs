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
        public EncoderListVMBase(ISimpleRepo<TDTO> repository, TArg appArguments, bool doReload = true) : base(repository, appArguments, doReload)
        {
            CanEncode = AppArgs.CanEncodeCollections(false);
        }


        public bool  CanEncode  { get; }
    }
}
