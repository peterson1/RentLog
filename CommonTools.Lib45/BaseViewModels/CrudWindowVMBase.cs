using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class CrudWindowVMBase<TDraft, TWindow, TArg> : MainWindowVMBase<TArg>
        where TWindow : Window, new()
        where TDraft  : Lib11.ReflectionTools.ICloneable, new()
        where TArg    : ICredentialsProvider
    {
        public event EventHandler<TDraft> NewRecordSaved          = delegate { };
        public event EventHandler<TDraft> RecordUpdated           = delegate { };
        public event EventHandler<TDraft> SaveCompleted           = delegate { };
        public event EventHandler<TDraft> SetupForInsertCompleted = delegate { };


        public CrudWindowVMBase(TArg appArguments) : base(appArguments)
        {
            EncodeNewDraftCmd = R2Command.Relay(EncodeNewDraft, _ => CanEncodeNewDraft(), $"Add New {TypeDescription}");
            SaveCompleted    += (s, e) => OnSaveCompleted();
        }


        public  IR2Command  EncodeNewDraftCmd  { get; }
        public  IR2Command  SaveDraftCmd       { get; private set; }
        public  TDraft      Draft              { get; private set; }
        public string       WhyInvalid         { get; protected set; }


        public    abstract string  TypeDescription  { get; }
        protected abstract bool    IsValidDraft     (TDraft draft, out string whyInvalid);

        public    virtual bool   CanEncodeNewDraft  ()             => true;
        protected virtual void   SaveNewRecord      (TDraft draft) { }
        protected virtual Task   SaveNewRecordAsync (TDraft draft) => Task.Delay(0);
        protected virtual void   UpdateRecord       (TDraft record) { }
        protected virtual Task   UpdateRecordAsync  (TDraft record) => Task.Delay(0);
        protected virtual TDraft GetNewDraft        () => new TDraft();

        protected virtual void   ModifyDraftForInserting   (TDraft draft) { }
        protected virtual Task   ModifyDraftForInsertAsync (TDraft draft) => Task.Delay(0);
        protected virtual void   ModifyDraftForUpdating  (TDraft draft) { }

        protected virtual void OnSaveCompleted    () { }


        public bool CanSave()
        {
            if (IsBusy) return false;
            if (!AllFieldsValid()) return false;
            if (Draft == null) return false;
            var ok = IsValidDraft(Draft, out string whyNot);
            WhyInvalid = whyNot;
            return ok;
        }


        private void EncodeNewDraft()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SetupForInsert();
#pragma warning restore CS4014
            if (Draft != null)
                ShowModalWindow();
        }


        public async Task SetupForInsert()
        {
            Draft = GetNewDraft();
            if (Draft == null) return;
            ModifyDraftForInserting(Draft);
            await ModifyDraftForInsertAsync(Draft);
            SaveDraftCmd = R2Command.Async(ExecuteSaveDraft, _ => CanSave(), $"Save {TypeDescription}");
            SetupForInsertCompleted?.Invoke(this, Draft);
        }


        protected virtual bool? ShowModalWindow()
            => this.Show<TWindow>(showModal: true);


        public bool? EditCurrentRecord(TDraft currentItem)
        {
            SetupForUpdate(currentItem);
            return ShowModalWindow();
        }


        public void SetupForUpdate(TDraft currentItem)
        {
            SaveDraftCmd = R2Command.Async(ExecuteUpdateRecord, _ => CanSave(), $"Save {TypeDescription}");
            Draft = CreateDraftFromRecord(currentItem);
            ModifyDraftForUpdating(Draft);
        }


        protected virtual TDraft CreateDraftFromRecord(TDraft record) 
            => record.ShallowClone<TDraft>();


        private async Task ExecuteSaveDraft()
        {
            if (!CanSave()) return;
            StartBeingBusy($"Saving new ‹{TypeDescription}› record ...");

            SaveNewRecord(Draft);
            await SaveNewRecordAsync(Draft);
            NewRecordSaved?.Invoke(this, Draft);

            AfterSaveCompleted(Draft);
        }


        private async Task ExecuteUpdateRecord()
        {
            if (!CanSave()) return;
            StartBeingBusy($"Updating ‹{TypeDescription}› record ...");

            UpdateRecord(Draft);
            await UpdateRecordAsync(Draft);
            RecordUpdated?.Invoke(this, Draft);

            AfterSaveCompleted(Draft);
        }


        protected virtual void AfterSaveCompleted(TDraft savedRecord)
        {
            SaveCompleted?.Invoke(this, savedRecord);
            ClearDraftAfterSave();
            StopBeingBusy();
            ReturnDialogResult(true);
        }


        protected virtual void ClearDraftAfterSave()
        {
            Draft = default(TDraft);
        }
    }
}
