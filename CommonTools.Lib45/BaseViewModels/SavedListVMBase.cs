using CommonTools.Lib11.DatabaseTools;
using CommonTools.Lib11.DataStructures;
using CommonTools.Lib11.DTOs;
using CommonTools.Lib11.GoogleTools;
using CommonTools.Lib11.InputCommands;
using CommonTools.Lib45.InputCommands;
using CommonTools.Lib45.ThreadTools;
using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib45.BaseViewModels
{
    [AddINotifyPropertyChangedInterface]
    public abstract class SavedListVMBase<TDTO, TArg> : IEnumerable<TDTO>
        where TDTO : IDocumentDTO
        where TArg : ICredentialsProvider
    {
        public event EventHandler<decimal> TotalSumChanged;
        public event EventHandler<TDTO>    SelectedChanged;

        private ISimpleRepo<TDTO> _repo;


        public SavedListVMBase(ISimpleRepo<TDTO> repository, TArg appArguments, bool doReload = true)
        {
            _repo         = repository;
            AppArgs       = appArguments;
            AddNewCmd     = R2Command.Relay(AddNewItem, _ => CanAddNewItem(), "Add New Item");
            RefreshCmd    = R2Command.Relay(ReloadFromDB, null, "Refresh");
            MainMethodCmd = R2Command.Relay(RunMainMethod, _ => PrivateCanRunMain(), MainMethodCmdLabel);

            ItemsList.ItemDeleted       += (s, e) => ExecuteDeleteRecord(e);
            ItemsList.ItemsDeleted      += (s, e) => ExecuteDeleteRecords(e);
            ItemsList.CollectionChanged += (s, e) => OnCollectionChanged();
            ItemsList.ItemsReplaced     += (s, e) => OnItemsReplaced();
            ItemsList.ItemOpened        += ItemsList_ItemOpened;
            if (_repo != null)
                _repo.ContentChanged += (s, e) => ReloadFromDB();

            if (doReload) ReloadFromDB();
        }


        public TArg          AppArgs         { get; }
        public IR2Command    AddNewCmd       { get; }
        public IR2Command    MainMethodCmd   { get; }
        public IR2Command    RefreshCmd      { get; }
        public UIList<TDTO>  ItemsList       { get; } = new UIList<TDTO>();
        public TDTO          Selected        { get; set; }
        public decimal       TotalSum        { get; private set; }
        public string        Caption         { get; protected set; }


        protected virtual string MainMethodCmdLabel => "Main Method";
        protected virtual Func<TDTO, decimal> SummedAmount { get; }
        protected virtual void AddNewItem          () { }
        protected virtual bool CanAddNewItem       () => true;
        protected virtual bool CanDeleteRecord     (TDTO rec) => true;
        public    virtual bool CanEditRecord       (TDTO rec) => true;
        protected virtual void LoadRecordForEditing(TDTO rec) { }
        protected virtual IEnumerable<TDTO> PostProcessQueried(IEnumerable<TDTO> items) => items;
        protected virtual void OnItemsReplaced     () { }
        protected virtual void OnSelectedChanged() => SelectedChanged?.Invoke(this, Selected);
        protected virtual void DeleteRecord  (ISimpleRepo<TDTO> db, TDTO dto) => db.Delete(dto);
        protected virtual void DeleteRecords (ISimpleRepo<TDTO> db, List<TDTO> items) => db.Delete(items);


        protected virtual bool  CanRunMainMethod  () => true;
        protected virtual void  RunMainMethod     () { }


        protected virtual void OnCollectionChanged()
        {
            UpdateTotalSum();
        }


        private bool PrivateCanRunMain()
        {
            if (!CanRunMainMethod()) return false;
            MainMethodCmd.CurrentLabel = MainMethodCmdLabel;
            return true;
        }


        protected bool CantDo(string whyNot)
        {
            MainMethodCmd.CurrentLabel = $"{MainMethodCmdLabel} -- {whyNot}";
            return false;
        }


        protected void ExecuteDeleteRecord(TDTO dto)
        {
            if (CanDeleteRecord(dto))
                DeleteRecord(_repo, dto);
            ReloadFromDB();
            UpdateTotalSum();
        }


        private void ExecuteDeleteRecords(List<TDTO> recordsToDelete)
        {
            var deletables = recordsToDelete.Where(_ 
                => CanDeleteRecord(_)).ToList();

            DeleteRecords(_repo, deletables);

            ReloadFromDB();
            UpdateTotalSum();
        }


        private void ItemsList_ItemOpened(object sender, TDTO e)
        {
            OnItemOpened(e);
            UpdateTotalSum();
        }


        protected virtual void OnItemOpened(TDTO e)
        {
            if (CanEditRecord(e))
                LoadRecordForEditing(e);
        }


        public virtual void ReloadFromDB()
        {
            var items = GetPostProcessedResult();
            UIThread.Run(() => ItemsList.SetItems(items));
        }

        protected IEnumerable<TDTO> GetPostProcessedResult()
            => PostProcessQueried(QueryItems(_repo));


        protected virtual List<TDTO> QueryItems(ISimpleRepo<TDTO> db)
            => db.GetAll();


        protected virtual bool TryGetPickedItem(out TDTO dto)
        {
            dto = ItemsList.CurrentItem;
            return dto != null;
        }


        protected void UpdateTotalSum()
        {
            if (SummedAmount == null) return;

            var oldSum = TotalSum;
            TotalSum = ItemsList.Any() 
                ? ItemsList.Sum(_ => SummedAmount(_)) : 0;

            if (TotalSum != oldSum)
                TotalSumChanged?.Invoke(this, TotalSum);
        }

        public IEnumerator<TDTO> GetEnumerator() => ((IEnumerable<TDTO>)ItemsList).GetEnumerator();
        IEnumerator IEnumerable .GetEnumerator() => ((IEnumerable<TDTO>)ItemsList).GetEnumerator();

        //public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
        //    => PropertyChanged?.Invoke(sender, e);

        public virtual void PersistUIList(bool doValidate)
        {
            //_repo.Drop();
            //_repo.Insert(ItemsList, doValidate);
            _repo.DropAndInsert(ItemsList, doValidate);
        }
    }
}
