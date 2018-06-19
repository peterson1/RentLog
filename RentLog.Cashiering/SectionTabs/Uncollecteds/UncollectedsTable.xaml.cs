using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.DTOs;
using RentLog.DomainLib45.SoaViewers.MainWindow;
using System.Windows.Controls;

namespace RentLog.Cashiering.SectionTabs.Uncollecteds
{
    public partial class UncollectedsTable : UserControl
    {
        public UncollectedsTable()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                dg.EnableOpenCurrent<UncollectedLeaseDTO>();
                dg.ConfirmToDelete<UncollectedLeaseDTO>(_ => _.Lease.TenantAndStall,
                    null, "Move [{0}] to “Did-Not-Operate”?", "Please confirm");
                dg.F4ToViewSoA<UncollectedLeaseDTO>(_ => _.Lease, VM.AppArgs);
                dg.ScrollToEndOnChange();
            };
        }


        private UncollectedsVM VM => DataContext as UncollectedsVM;
    }
}
