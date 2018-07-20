using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.TenantCollections
{
    public partial class TenantColxnsTable : UserControl
    {
        public TenantColxnsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.CurrentCellChanged += (c, d)
                    => dg.SelectedIndex = -1;
            };
        }
    }
}
