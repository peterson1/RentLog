using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.StallsInventory
{
    public partial class StallsInventoryUI1 : UserControl
    {
        public StallsInventoryUI1()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                tblTotals.dg.HeadersVisibility = DataGridHeadersVisibility.None;
            };
        }
    }
}
