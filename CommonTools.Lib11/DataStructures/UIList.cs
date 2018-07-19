using CommonTools.Lib11.CollectionTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CommonTools.Lib11.DataStructures
{
    public class UIList<T> : ObservableCollection<T>
    {
        public event EventHandler<T>       ItemOpened    = delegate { };
        public event EventHandler<T>       ItemDeleted   = delegate { };
        public event EventHandler<List<T>> ItemsDeleted  = delegate { };
        public event EventHandler          ItemsReplaced = delegate { };


        public UIList() { }

        public UIList(IEnumerable<T> rowItems)
        {
            SetItems(rowItems);
        }

        public UIList(IEnumerable<T> rowItems, T summaryRow, double summaryAmount)
        {
            SetItems       (rowItems);
            SetSummary     (summaryRow);
            SummaryAmount = summaryAmount;
        }


        public double  SummaryAmount    { get; set; }
        public T       CurrentItem      { get; set; }
        public bool    IsReplacingItems { get; private set; }


        public ObservableCollection<T>  SummaryRows  { get; } = new ObservableCollection<T>();


        public void RaiseItemOpened(T item)
        {
            if (item != null)
                ItemOpened?.Invoke(this, item);
        }


        public void RaiseItemDeleted(T item)
        {
            if (item != null)
                ItemDeleted?.Invoke(this, item);
        }


        public void RaiseItemsDeleted(List<T> items)
        {
            if (items != null)
                ItemsDeleted?.Invoke(this, items);
        }


        //public void AskToDeleteCurrent(string message, Action deleteAction, string caption = "Confirm to Delete")
        //    => Alert


        public void SetItems(IEnumerable<T> items)
        {
            if (IsReplacingItems) return;
            IsReplacingItems = true;

            IgnoreUIErrors(() => this.Clear());
            if (items != null)
            {
                foreach (var item in items)
                    IgnoreUIErrors(() => this.Add(item));
            }

            IgnoreUIErrors(() 
                => ItemsReplaced?.Invoke(this, EventArgs.Empty));

            IsReplacingItems = false;
        }


        private void IgnoreUIErrors(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (ArgumentException) { }
        }



        public void RemoveAll(Predicate<T> match)
        {
            var list = this.ToList();
            list.RemoveAll(match);
            SetItems(list);
        }


        public void SetSummary(T summaryRow)
        {
            SummaryRows.Clear();
            SummaryRows.Add(summaryRow);
        }
    }
}
