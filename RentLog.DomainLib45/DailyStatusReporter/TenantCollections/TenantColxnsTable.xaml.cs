using RentLog.DomainLib11.Reporters;
using RentLog.DomainLib11.ReportRows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RentLog.DomainLib45.DailyStatusReporter.TenantCollections
{
    public partial class TenantColxnsTable : UserControl
    {
        public TenantColxnsTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.LostFocus += (c, d) => dg.SelectedIndex = -1;
                dg.MouseDoubleClick += Dg_MouseDoubleClick;
            };
        }


        private void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dg.SelectedIndex == -1) return;
            if (dg.SelectedItems.Count != 1) return;
            if (dg.SelectedItem is SectionColxnsRow row)
                SectionRowInspectorVM.Launch(row, VM.Sources);
        }


        private DailyColxnsReport VM => DataContext as DailyColxnsReport;
    }
}
