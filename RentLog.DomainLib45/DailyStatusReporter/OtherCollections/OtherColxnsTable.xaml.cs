using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.OtherCollections
{
    public partial class OtherColxnsTable : UserControl
    {
        public OtherColxnsTable()
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
