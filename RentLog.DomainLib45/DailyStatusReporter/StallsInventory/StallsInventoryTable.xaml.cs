using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.StallsInventory
{
    public partial class StallsInventoryTable : UserControl
    {
        public StallsInventoryTable()
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
