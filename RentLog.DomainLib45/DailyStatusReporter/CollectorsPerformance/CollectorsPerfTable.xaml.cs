using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.CollectorsPerformance
{
    public partial class CollectorsPerfTable : UserControl
    {
        public CollectorsPerfTable()
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
