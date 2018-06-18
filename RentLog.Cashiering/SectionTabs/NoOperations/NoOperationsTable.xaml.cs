using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs.NoOperations
{
    public partial class NoOperationsTable : UserControl
    {
        public NoOperationsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.ConfirmToDelete<UncollectedLeaseDTO>(_ => _.Lease.TenantAndStall,
                    null, "Move [{0}] to “Uncollected”?", "Please confirm");
            };
        }
    }
}
