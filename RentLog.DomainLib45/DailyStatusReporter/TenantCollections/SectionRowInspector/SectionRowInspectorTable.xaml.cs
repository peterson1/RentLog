using CommonTools.Lib45.UIExtensions;
using RentLog.DomainLib11.ReportRows;
using System.Windows.Controls;

namespace RentLog.DomainLib45.DailyStatusReporter.TenantCollections.SectionRowInspector
{
    public partial class SectionRowInspectorTable : UserControl
    {
        public SectionRowInspectorTable()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                dg.EnableOpenCurrent<LeaseColxnRow>();
            };
        }
    }
}
