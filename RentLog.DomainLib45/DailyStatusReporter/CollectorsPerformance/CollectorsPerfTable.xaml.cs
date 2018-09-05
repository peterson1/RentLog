using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib11.ReportRows;
using RentLog.DomainLib45.DailyStatusReporter.CollectorsPerformance.MainRowInspector;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.DomainLib45.DailyStatusReporter.CollectorsPerformance
{
    public partial class CollectorsPerfTable : UserControl
    {
        public CollectorsPerfTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                //dg.CurrentCellChanged += (c, d)
                //    => dg.SelectedIndex = -1;
                dg.LostFocus += (c, d) => dg.SelectedIndex = -1;
                dg.MouseDoubleClick += Dg_MouseDoubleClick;
            };
        }


        private void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dg.SelectedIndex == -1) return;
            if (dg.SelectedItems.Count != 1) return;

            var rep = DataContext as DailyStatusReport;

            if (dg.SelectedItem is CollectorPerformanceRow row)
                MainRowInspectorVM.Launch(row, rep.DBsDir);
        }
    }
}
