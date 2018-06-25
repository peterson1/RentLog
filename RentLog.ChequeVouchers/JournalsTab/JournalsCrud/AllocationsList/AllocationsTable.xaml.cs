using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.Models;
using System.Windows.Controls;

namespace RentLog.ChequeVouchers.JournalsTab.JournalsCrud.AllocationsList
{
    public partial class AllocationsTable : UserControl
    {
        public AllocationsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<AccountAllocation>();
                dg.ConfirmToDelete<AccountAllocation>(_ => _.Account.Name);
            };
        }
    }
}
