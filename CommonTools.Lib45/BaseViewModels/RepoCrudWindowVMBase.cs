using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System.Windows;

namespace CommonTools.Lib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class RepoCrudWindowVMBase<TRepo, TDraft, TWindow, TArg> 
        : CrudWindowVMBase<TDraft, TWindow, TArg>
        where TRepo   : ISimplerRepo<TDraft>
        where TWindow : Window, new()
        where TDraft  : Lib11.ReflectionTools.ICloneable, new()
        where TArg    : ICredentialsProvider
    {
        protected TRepo _repo;


        public RepoCrudWindowVMBase(TRepo repository, TArg appArguments) : base(appArguments)
        {
            _repo = repository;
        }


        protected override void SaveNewRecord(TDraft draft)
            => _repo.Insert(draft);


        protected override void UpdateRecord(TDraft record)
        {
            if (!_repo.Update(record))
            {
                var recText = (record is IDocumentDTO dto)
                            ? $"record ID [{dto.Id}]" : $"“{record}”";

                Alert.ShowModal("Failed to update record.", $"Unable to save {recText}.");
            }
        }

        protected override bool IsValidDraft(TDraft draft, out string whyInvalid)
        {
            if (draft is IDocumentDTO doc)
            {
                if (doc.Id == 0)
                {
                    if (!_repo.IsValidForInsert(draft, out whyInvalid)) return false;
                }
                else
                {
                    if (!_repo.IsValidForUpdate(draft, out whyInvalid)) return false;
                }
            }

            whyInvalid = string.Empty;
            return true;
        }
    }
}
